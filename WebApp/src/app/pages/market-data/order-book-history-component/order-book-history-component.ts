import { CommonModule } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';
import { FormControl } from '@angular/forms';
import { OrderBookApiService } from '../shared/order-book-api-service';
import { DepthSnapshot } from '../model/depth.model';
import { ChartComponent } from '../../../shared/chart-component/chart-component';

@Component({
  selector: 'app-order-book-history-component',
  imports: [CommonModule, SharedModule],
  templateUrl: './order-book-history-component.html',
  styleUrl: './order-book-history-component.css',
})
export class OrderBookHistoryComponent {

  @ViewChild(ChartComponent) chartComponent?: ChartComponent;

  selectedDate = new FormControl(new Date());
  selectedTime = new FormControl(new Date());

  currentSnapshot?: DepthSnapshot;

  constructor(private orderBookApiService: OrderBookApiService) {
  }

  private mergeDateTime(): string {
    const date = this.selectedDate.value || new Date();
    const time = this.selectedTime.value || new Date();

    const year = date.getFullYear();
    const month = date.getMonth();
    const day = date.getDate();
    const hours = time.getHours();
    const minutes = time.getMinutes();

    const utcDate = new Date(Date.UTC(year, month, day, hours, minutes, 0, 0));

    return utcDate.toISOString();
  }

  getHistoricalData() {
    const dateTime = this.mergeDateTime();
    this.orderBookApiService.getHistoricalData(dateTime).subscribe({
      next: (data) => {
        if (this.chartComponent) {
          this.chartComponent.updateChart(data);
        }
      },
      error: (e) => {
        if (this.chartComponent) {
          this.chartComponent.clearChart();
        }
      }
    })
  }
}
