import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../core/base-api-service';
import { DepthSnapshot } from '../model/depth.model';
import { ENDPOINTS } from '../../../core/api-endpoints';

@Injectable({
  providedIn: 'root',
})
export class OrderBookApiService extends BaseApiService<DepthSnapshot> {
  protected override endpoint: string = ENDPOINTS.ORDER_BOOK;
}
