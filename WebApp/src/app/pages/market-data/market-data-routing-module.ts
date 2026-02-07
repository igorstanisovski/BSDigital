import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderBookComponent } from './order-book-component/order-book-component';
import { OrderBookHistoryComponent } from './order-book-history-component/order-book-history-component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'live'
  },
  {
    path: 'live',
    component: OrderBookComponent
  },
  {
    path: 'history',
    component: OrderBookHistoryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MarketDataRoutingModule { }
