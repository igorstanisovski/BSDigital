import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../utils/material.module';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ChartComponent } from './chart-component/chart-component';

const THIRD_MODULES = [
  MaterialModule,
  FormsModule,
  ReactiveFormsModule
]

const COMPONENTS = [
  ChartComponent
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule,
    ...THIRD_MODULES,
    ...COMPONENTS
  ],
  exports: [
    RouterModule,
    ...THIRD_MODULES,
    ...COMPONENTS
  ]
})
export class SharedModule { }
