import { Injectable } from '@angular/core';
import { SignalRClientService } from '../../../core/signal-rclient-service';
import { DepthSnapshot } from '../model/depth.model';
import { BehaviorSubject, Subscription } from 'rxjs';
import { environment } from '../../../../environment/environment.dev';
import { Subject } from 'rxjs';
import { ConnectionStatus } from '../../../core/connection-status.enum';

@Injectable({
  providedIn: 'root',
})
export class OrderBookService {
  private client: SignalRClientService;
  public depth$ = new BehaviorSubject<DepthSnapshot | null>(null);
  public quote$ = new BehaviorSubject<number | null>(null);
  private readonly BASE_URL: string = environment.BASE_URL

  public connectionStatus$ = new Subject<ConnectionStatus>();
  public onConnectionStatus$ = this.connectionStatus$.asObservable();
  private errorSubscription?: Subscription;
  private reconnectingSubscription?: Subscription;
  private reconnectedSubscription?: Subscription;
  private closedSubscription?: Subscription;

  constructor() {
    this.client = new SignalRClientService(`${this.BASE_URL}/market-data`);
    this.setupErrorHandling();
  }

  private setupErrorHandling(): void {
    this.errorSubscription = this.client.onConnectionError$.subscribe(error => {
      this.connectionStatus$.next(ConnectionStatus.Error);
    });

    this.reconnectingSubscription = this.client.onReconnecting$.subscribe(() => {
      this.connectionStatus$.next(ConnectionStatus.Reconnecting);
    });

    this.reconnectedSubscription = this.client.onReconnected$.subscribe(() => {
      this.connectionStatus$.next(ConnectionStatus.Reconnecting);
    });

    this.closedSubscription = this.client.onClosed$.subscribe((error) => {
      if (error) {
        this.connectionStatus$.next(ConnectionStatus.Error);
      } else {
        this.connectionStatus$.next(ConnectionStatus.Disconnected);
      }
    });
  }

  connect(): void {
    this.connectionStatus$.next(ConnectionStatus.Connecting);
    this.client.start()
      .then(() => {
        this.connectionStatus$.next(ConnectionStatus.Connected);
        this.listenForDepth();
        this.listenForQuote();
      })
      .catch(err => {
        this.connectionStatus$.next(ConnectionStatus.Error);
      });
  }

  disconnect(): void {
    this.client.stop();
    this.errorSubscription?.unsubscribe();
    this.reconnectingSubscription?.unsubscribe();
    this.reconnectedSubscription?.unsubscribe();
    this.closedSubscription?.unsubscribe();
  }

  listenForDepth(): void {
    this.client.on<DepthSnapshot>('DepthUpdate', snapshot => {
      this.depth$.next(snapshot);
    });
  }

  listenForQuote(): void {
    this.client.on<number>('QuoteUpdated', value => {
      this.quote$.next(value);
    });
  }

  invokeBtcChange(btcAmount: number): void {
    this.client.invoke<void>('SetBtcAmount', btcAmount)
  }
}
