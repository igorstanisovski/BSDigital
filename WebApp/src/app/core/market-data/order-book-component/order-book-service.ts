import { Injectable, OnInit } from '@angular/core';
import { SignalRClientService } from '../../signal-rclient-service';
import { DepthSnapshot } from '../model/depth.model';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../../environment/environment.dev';

@Injectable({
  providedIn: 'root',
})
export class OrderBookService {
  private client: SignalRClientService;
  public depth$ = new BehaviorSubject<DepthSnapshot | null>(null);
  public quote$ = new BehaviorSubject<number | null>(null);
  private readonly BASE_URL: string = environment.BASE_URL

  constructor() {
    this.client = new SignalRClientService(`${this.BASE_URL}/market-data`);
  }

  async connect() {
    await this.client.start();
    this.listenForDepth();
    this.listenForQuote();
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
