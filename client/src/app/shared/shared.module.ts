import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TextInputComponent } from './components/text-input/text-input.component';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { StepperComponent } from './components/stepper/stepper.component';
import { BasketSummaryComponent } from './components/basket-summary/basket-summary.component';
import { RouterModule } from '@angular/router';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { PhotoWidgetComponent } from './components/photo-widget/photo-widget.component';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { ImageCropperModule } from 'ngx-image-cropper';
import { NgxPaginationModule } from 'ngx-pagination';

@NgModule({
  declarations: [
    PagingHeaderComponent,
    PagerComponent,
    OrderTotalsComponent,
    TextInputComponent,
    StepperComponent,
    BasketSummaryComponent,
    PhotoWidgetComponent
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    BsDropdownModule.forRoot(),
    PaginationModule.forRoot(),
    CarouselModule.forRoot(),
    CdkStepperModule,
    RouterModule,
    CurrencyMaskModule,
    NgxGalleryModule,
    TabsModule.forRoot(),
    NgxDropzoneModule,
    ImageCropperModule,
    NgxPaginationModule
  ],
  exports: [
    FormsModule,
    PaginationModule, 
    PagingHeaderComponent,
    PagerComponent,
    CarouselModule,
    OrderTotalsComponent,
    ReactiveFormsModule,
    BsDropdownModule,
    TextInputComponent,
    CdkStepperModule,
    StepperComponent,
    BasketSummaryComponent,
    CurrencyMaskModule,
    NgxGalleryModule,
    TabsModule,
    PhotoWidgetComponent,
    NgxDropzoneModule,
    ImageCropperModule,
    NgxPaginationModule
  ]
})
export class SharedModule { }
