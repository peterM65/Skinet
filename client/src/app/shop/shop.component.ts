import { ShopService } from './shop.service';
import { Component, OnInit } from '@angular/core';
import { Product } from '../shared/models/product';
import { Brand } from '../shared/models/brands';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  products : Product [] = [];
  brands: Brand [] = [];
  types: Type [] = [];
  shopParams = new ShopParams();
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low To High', value: 'priceAsc'},
    {name: 'Price: Hidh To Low', value: 'priceDesc'}
  ];
  totalCount = 0 ;

  constructor(private ShopService:ShopService){}

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }
  getProducts(){
    this.ShopService.getProducts(this.shopParams).subscribe({
      next: response => {
        this.products = response.data;
        this.shopParams.pageNumber = response.pageIndex;
        this.shopParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: error => console.log(error),
      })
  }
  getBrands(){
    this.ShopService.getBrands().subscribe({
      next: response => this.brands = [{id:0, name:'All'}, ...response],
      error: error => console.log(error),
      })
  }
  getTypes(){
    this.ShopService.getTypes().subscribe({
      next: response => this.types = [{id:0, name:'All'}, ...response],
      error: error => console.log(error),
      })
  }

  onBrandSelected(brandId: number){
    this.shopParams.brandId = brandId;
    this.getProducts();
  }

  onTypeSelected(TypeId: number){
    this.shopParams.typeId = TypeId;
    this.getProducts();
  }

  onSortSelected(event: any){
    this.shopParams.sort = event.target.value;
    this.getProducts();
  }

  onPageChanged(event: any){
    if(this.shopParams.pageNumber != event.page){
    this.shopParams.pageNumber = event.page;
    this.getProducts();
    }
  }
}
