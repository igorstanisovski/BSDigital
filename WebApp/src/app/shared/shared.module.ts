import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../utils/material.module';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

const THIRD_MODULES = [
  MaterialModule,
  FormsModule,
  ReactiveFormsModule
]

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule,
    ...THIRD_MODULES
  ],
  exports: [
    RouterModule,
    ...THIRD_MODULES
  ]
})
export class SharedModule { }
