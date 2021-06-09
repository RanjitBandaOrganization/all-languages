import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { tap, map, catchError  } from 'rxjs/operators';
import { IProduct } from './product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

    _productURL: string = './/..//..//data/products/products.json';
    constructor(private _http: HttpClient){
    }

    getProducts(): Observable<IProduct[]> {
        return this._http.get<IProduct[]>(this._productURL)
                .pipe(tap(ev => console.log(ev)), map(x => x))
                // .catchError(this.handleError);
    }
    
    private handleError(err: HttpErrorResponse) {
        console.error(err.message);
        return throwError("err.message");
    }
}
