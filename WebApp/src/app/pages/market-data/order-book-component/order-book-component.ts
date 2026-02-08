import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { OrderBookService } from '../shared/order-book-service';
import { DepthPoint, DepthSnapshot } from '../model/depth.model';
import { SharedModule } from '../../../shared/shared.module';
import { Subscription } from 'rxjs';
import { ChangeDetectorRef } from '@angular/core';
import { ChartComponent } from '../../../shared/chart-component/chart-component';
import { ConnectionStatus } from '../../../core/connection-status.enum';

@Component({
  selector: 'app-order-book-component',
  imports: [CommonModule, SharedModule, ChartComponent],
  templateUrl: './order-book-component.html',
  styleUrl: './order-book-component.css',
})
export class OrderBookComponent implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild(ChartComponent) chartComponent?: ChartComponent;

  btcAmount: number = 0;
  quote: number | null = null;
  connectionStatus: string = ConnectionStatus.Disconnected;

  asks: DepthPoint[] = [];
  currentSnapshot?: DepthSnapshot;

  private depthSubscription?: Subscription;
  private quoteSubscription?: Subscription;
  private connectionStatusSubscription?: Subscription;

  constructor(
    private orderBookService: OrderBookService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.connectionStatusSubscription = this.orderBookService.onConnectionStatus$.subscribe(status => {
      this.connectionStatus = status;
      this.cdr.markForCheck();
    });

    this.depthSubscription = this.orderBookService.depth$.subscribe(snapshot => {
      if (!snapshot) return;

      this.currentSnapshot = snapshot;
      this.asks = snapshot.asks;

      if (this.chartComponent) {
        this.chartComponent.updateChart(snapshot);
      }
    });

    this.quoteSubscription = this.orderBookService.quote$.subscribe(value => {
      if (!value || value === 0) {
        this.quote = null;
      } else {
        this.quote = value;
      }
      this.cdr.markForCheck();
    });

    this.orderBookService.connect();
  }

  ngAfterViewInit(): void {
    if (this.currentSnapshot && this.chartComponent) {
      this.chartComponent.updateChart(this.currentSnapshot);
    }
  }

  ngOnDestroy(): void {
    this.depthSubscription?.unsubscribe();
    this.quoteSubscription?.unsubscribe();
    this.connectionStatusSubscription?.unsubscribe();
    this.orderBookService.disconnect();
  }

  onBtcAmountChange(): void {
    this.orderBookService.invokeBtcChange(this.btcAmount || 0)
  }
}