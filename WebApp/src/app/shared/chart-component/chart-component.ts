import { Component, ElementRef, Input, OnChanges, OnDestroy, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Chart, ChartConfiguration } from 'chart.js';
import { CommonModule } from '@angular/common';
import { DepthSnapshot } from '../../pages/market-data/model/depth.model';

@Component({
  selector: 'app-chart-component',
  imports: [CommonModule],
  templateUrl: './chart-component.html',
  styleUrl: './chart-component.css',
})
export class ChartComponent implements OnInit, OnDestroy {
  private chart!: Chart;

  @Input() snapshot?: DepthSnapshot;
  @ViewChild('chartCanvas', { static: true }) chartCanvas!: ElementRef<HTMLCanvasElement>;

  ngOnInit() {
    this.chart = new Chart(this.chartCanvas.nativeElement, this.getChartConfig());
  }

  ngOnDestroy(): void {
    if (this.chart) {
      this.chart.destroy();
      this.chart = null as any;
    }
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
        responsive: true,
        maintainAspectRatio: true,
        aspectRatio: 2,
        scales: {
          x: { type: 'linear', title: { display: true, text: 'Price (EUR)' } },
          y: { title: { display: true, text: 'Cumulative Volume' }, beginAtZero: true, max: 40 }
        },
      }
    };
  }

  public updateChart(snapshot: DepthSnapshot) {
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

  public clearChart(): void {
    if (!this.chart) return;

    this.chart.data.datasets[0].data = [];
    this.chart.data.datasets[1].data = [];
    this.chart.update('none');
  }
}
