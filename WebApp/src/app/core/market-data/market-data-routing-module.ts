import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderBookComponent } from './order-book-component/order-book-component';

const routes: Routes = [
  {
    path: '',
    component: OrderBookComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MarketDataRoutingModule { }
