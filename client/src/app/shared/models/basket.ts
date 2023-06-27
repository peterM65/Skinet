export interface BasketItem {
  id:         number;
  name:       null;
  price:      number;
  quantity:   number;
  pictureUrl: string;
  brand:      string;
  type:       string;
}

export interface Basket {
  id:    string;
  items: BasketItem[];
}

export class Basket implements Basket{
  id = 'sdsdsdsdsdsd';
  items: BasketItem[] = [];
}

