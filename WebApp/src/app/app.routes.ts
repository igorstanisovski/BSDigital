import { Routes } from '@angular/router';
import { HomeComponent } from './layout/home-component/home-component';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        component: HomeComponent,
        children: [
            {
                path: '',
                pathMatch: 'full',
                redirectTo: 'order-book'
            },
            {
                path: 'order-book',
                loadChildren: () => import('./core/market-data/market-data-module').then((m) => m.MarketDataModule)
            }
        ]
    },
    {
        path: '**', redirectTo: ''
    }
];
