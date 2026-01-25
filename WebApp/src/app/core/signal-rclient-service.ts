import * as signalR from '@microsoft/signalr';

export class SignalRClientService {
  private hubConnection!: signalR.HubConnection;

  constructor(private hubUrl: string) {}

  start(): Promise<void> {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl)
      .withAutomaticReconnect()
      .build();

    return this.hubConnection.start()
      .then(() => console.log(`SignalR connected to ${this.hubUrl}`))
      .catch(err => console.error('SignalR connection error:', err));
  }

  stop() {
    return this.hubConnection.stop();
  }

  on<T>(eventName: string, handler: (data: T) => void): void {
    this.hubConnection.on(eventName, handler);
  }
}