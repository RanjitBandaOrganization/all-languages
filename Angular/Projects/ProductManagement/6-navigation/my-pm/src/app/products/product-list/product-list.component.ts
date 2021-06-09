import { Component, OnInit } from '@angular/core';
import { IProduct } from '../product';
import { ProductService } from '../product.service';

@Component({
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {

  constructor(private _productService: ProductService) {
    /* setting default values */
   }

  ngOnInit(): void {
    this._productService.getProducts()
    .subscribe(prod => {
      this.products = prod
      console.log("test");
    this.filteredProducts = this.products;
    },
      error => this.errorMessage = <any>error);
  }

  errorMessage: string;
  pageTitle: string = 'Product List';
  showImage: boolean = false;
  _filterBy: string;

  get filterBy() {
    return this._filterBy;
  }
  set filterBy(value: string) {
    this._filterBy = value;
    this.filteredProducts = (this.filterBy ? this.performProductsFilter(this.filterBy) : this.products);
  }
  performProductsFilter(filterByString: string): IProduct[] {
    filterByString = filterByString.toLocaleLowerCase();
   return this.products.filter((product: IProduct) => product.productName.toLocaleLowerCase().indexOf(filterByString) !== -1) ;
  }
  filteredProducts = [];

  products: IProduct[] =  [];

  toggleImage(): void{
    this.showImage = !this.showImage;
  }

  onRatingClicked(message: string): void{
    this.pageTitle = message;
  }

}
