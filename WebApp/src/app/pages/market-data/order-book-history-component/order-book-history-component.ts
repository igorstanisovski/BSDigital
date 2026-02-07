import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SharedModule } from '../../../shared/shared.module';
import { FormControl } from '@angular/forms';
import { OrderBookApiService } from '../shared/order-book-api-service';

@Component({
  selector: 'app-order-book-history-component',
  imports: [CommonModule, SharedModule],
  templateUrl: './order-book-history-component.html',
  styleUrl: './order-book-history-component.css',
})
export class OrderBookHistoryComponent {
  selectedDate = new FormControl(new Date());
  selectedTime = new FormControl('12:00');

  constructor(private orderBookApiService: OrderBookApiService) {}

  getHistoricalData() {
    
  }

}
