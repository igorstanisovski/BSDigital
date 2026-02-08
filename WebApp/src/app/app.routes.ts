import { Routes } from '@angular/router';
import { HomeComponent } from './layout/home-component/home-component';
import { WildcardComponent } from './pages/wildcard-component/wildcard-component';

export const routes: Routes = [
    {
        path: '',
        component: HomeComponent,
        children: [
            {
                path: '',
                pathMatch: 'full',
                redirectTo: 'order-book'
            },
            {
                path: 'order-book',
                loadChildren: () => import('./pages/market-data/market-data-module').then((m) => m.MarketDataModule)
            }
        ]
    },
    {
        path: '**', 
        component: WildcardComponent
    }
];
