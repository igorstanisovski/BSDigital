import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../utils/material.module';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

const THIRD_MODULES = [
  MaterialModule,
  FormsModule
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
