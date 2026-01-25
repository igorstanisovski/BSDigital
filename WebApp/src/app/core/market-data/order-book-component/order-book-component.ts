import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { OrderBookService } from './order-book-service';
import { Chart, ChartConfiguration } from 'chart.js';
import { DepthPoint, DepthSnapshot } from '../model/depth.model';
import { SharedModule } from '../../../shared/shared.module';
import { Subject, Subscription, throttleTime } from 'rxjs';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-order-book-component',
  imports: [CommonModule, SharedModule],
  templateUrl: './order-book-component.html',
  styleUrl: './order-book-component.css',
})
export class OrderBookComponent implements OnInit, OnDestroy {

  private chart!: Chart;

  btcAmount: number = 0;
  quote: number | null = null;

  asks: DepthPoint[] = [];

  private snapshot$ = new Subject<DepthSnapshot>();
  private snapshotSub!: Subscription;

  constructor(
    private orderBookService: OrderBookService,
    private cdr : ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.chart = new Chart('depthChart', this.getChartConfig());
    this.orderBookService.connect();
    
    this.orderBookService.depth$.subscribe(snapshot => {
      if (!snapshot) return;

      this.snapshot$.next(snapshot);

      this.updateChart(snapshot);
      this.asks = snapshot.asks;
    });

    // Throttle quote calculation to every 200ms
    this.snapshotSub = this.snapshot$
      .pipe(throttleTime(200))
      .subscribe(() => {
        if (this.btcAmount > 0) {
          this.calculateQuote();
          this.cdr.markForCheck();
        }
      });
  }

   ngOnDestroy(): void {
    this.snapshotSub.unsubscribe();
  }

  private getChartConfig(): ChartConfiguration {
    return {
      type: 'line',
      data: {
        datasets: [
          { label: 'Bids', data: [], borderColor: 'green', fill: true },
          { label: 'Asks', data: [], borderColor: 'red', fill: true },
        ]
      },
      options: {
        animation: false,
        scales: {
          x: { type: 'linear', title: { display: true, text: 'Price (EUR)' } },
          y: { title: { display: true, text: 'Cumulative Volume' }, beginAtZero: true, max: 40 }
        },
      }
    };
  }

  calculateQuote() {
    if (!this.asks || this.asks.length === 0 || this.btcAmount <= 0) {
      this.quote = null;
      return;
    }

    let remaining = this.btcAmount;
    let totalCost = 0;

    for (const ask of this.asks) {
      const availableAtLevel = ask.cumulative;
      const prev = totalCost / ask.price;
      const take = Math.min(remaining, availableAtLevel - prev);

      totalCost += take * ask.price;
      remaining -= take;

      if (remaining <= 0) break;
    }

    this.quote = remaining <= 0 ? totalCost : null;
  }

  private updateChart(snapshot: DepthSnapshot) {
    const bids = snapshot.bids.map(l => ({ x: l.price, y: l.cumulative }));
    const asks = snapshot.asks.map(l => ({ x: l.price, y: l.cumulative }));

    this.chart.data.datasets[0].data = bids;
    this.chart.data.datasets[1].data = asks;

    const prices = [...bids, ...asks].map(p => p.x);
    const xMin = Math.min(...prices);
    const xMax = Math.max(...prices);

    (this.chart.options.scales!['x'] as any).min = xMin;
    (this.chart.options.scales!['x'] as any).max = xMax;

    this.chart.update('none');
  }
}
