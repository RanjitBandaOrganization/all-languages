import { Component, OnInit } from '@angular/core';
import { IProduct } from '../product';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {

  constructor() {
    /* setting default values */
    this.filteredProducts = this.products;
    this.filterBy = 'cart';
   }

  ngOnInit(): void {
  }

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

  products: IProduct[] =  [
      {
          "productId": 2,
          "productName": "Garden Cart",
          "productCode": "GDN-0023",
          "releaseDate": "March 18, 2016",
          "description": "15 gallon capacity rolling garden cart",
          "price": 32.99,
          "starRating": 4.2,
          "imageUrl": "http://openclipart.org/image/300px/svg_to_png/58471/garden_cart.png"
      },
      {
          "productId": 5,
          "productName": "Hammer",
          "productCode": "TBX-0048",
          "releaseDate": "May 21, 2016",
          "description": "Curved claw steel hammer",
          "price": 8.9,
          "starRating": 4.8,
          "imageUrl": "http://openclipart.org/image/300px/svg_to_png/73/rejon_Hammer.png"
      }
  ];

  toggleImage(): void{
    this.showImage = !this.showImage;
  }

  onRatingClicked(message: string): void{
    this.pageTitle = message;
  }

}
