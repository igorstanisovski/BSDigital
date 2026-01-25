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
  private readonly BASE_URL: string = environment.BASE_URL

  constructor() {
    this.client = new SignalRClientService(`${this.BASE_URL}/market-data`);
  }

  async connect() {
    await this.client.start();

    this.client.on<DepthSnapshot>('DepthUpdate', snapshot => {
      this.depth$.next(snapshot);
    });
  }
}
