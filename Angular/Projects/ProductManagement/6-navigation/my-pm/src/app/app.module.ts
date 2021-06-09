import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { ProductListComponent } from './products/product-list/product-list.component';
import { ProductService } from './products/product.service';
import { ProductGuardService } from './products/product.guard.service';
import { StarComponent } from './shared/star.component';
import { RouterModule } from '@angular/router';
import { ProductDetailComponent } from './products/product-detail/product-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    ProductListComponent,
    StarComponent,
    ProductDetailComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
        { path: 'products', component: ProductListComponent },
        { path: 'products/:id', canActivate: [ProductGuardService], component: ProductDetailComponent },
        { path: '', redirectTo: 'products', pathMatch: 'full'},
        { path: '**', redirectTo: 'products', pathMatch: 'full'}
    ])
  ],
  providers: [ProductService, ProductGuardService],
  bootstrap: [AppComponent]
})
export class AppModule { }
