import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';

export class SignalRClientService {
  private hubConnection!: signalR.HubConnection;

  private connectionError$ = new Subject<string>();
  private reconnecting$ = new Subject<Error | undefined>();
  private reconnected$ = new Subject<string | undefined>();
  private closed$ = new Subject<Error | undefined>();

  public onConnectionError$ = this.connectionError$.asObservable();
  public onReconnecting$ = this.reconnecting$.asObservable();
  public onReconnected$ = this.reconnected$.asObservable();
  public onClosed$ = this.closed$.asObservable();

  constructor(private hubUrl: string) { }

  start(): Promise<void> {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl)
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (retryContext) => {
          // Retry every 2 seconds for up to 5 attempts
          if (retryContext.previousRetryCount < 5) {
            return 2000;
          } else {
            // Stop after 5 retries
            return null;
          }
        }
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.setupEventHandlers();

    return this.hubConnection.start()
      .then(() => console.log(`SignalR connected to ${this.hubUrl}`))
      .catch(err => {
        console.error('SignalR connection error:', err);
        this.connectionError$.next(err.message || 'Connection failed');
        throw err;
      });
  }

  private setupEventHandlers(): void {
    this.hubConnection.onreconnecting((error) => {
      this.reconnecting$.next(error);
    });

    this.hubConnection.onreconnected((connectionId) => {
      this.reconnected$.next(connectionId);
    });

    this.hubConnection.onclose((error) => {
      if (error) {
        this.connectionError$.next(error.message);
      }
      this.closed$.next(error);
    });
  }

  stop(): Promise<void> {
    if (this.hubConnection) {
      return this.hubConnection.stop();
    }
    return Promise.resolve();
  }

  on<T>(eventName: string, handler: (data: T) => void): void {
    this.hubConnection.on(eventName, handler);
  }

  invoke<T>(methodName: string, ...args: any[]): Promise<T> {
    if (!this.hubConnection || this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      const error = 'Hub connection not established or disconnected';
      this.connectionError$.next(error);
      return Promise.reject(error);
    }

    return this.hubConnection.invoke<T>(methodName, ...args)
      .catch((error) => {
        this.connectionError$.next(error.message || `Failed to invoke ${methodName}`);
        throw error;
      });
  }

  isConnected(): boolean {
    return this.hubConnection?.state === signalR.HubConnectionState.Connected;
  }

  getConnectionState(): signalR.HubConnectionState | undefined {
    return this.hubConnection?.state;
  }
}