import { Component, OnInit, Input, ViewChildren, QueryList  } from '@angular/core';
import { ItemComponent } from '../item/item.component';
import { JasminService } from '../../services/jasmin.service';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent implements OnInit {
  @ViewChildren('com') itemComponents:QueryList<ItemComponent>;

  private items:any;

  constructor(private jasminService: JasminService) { }

  ngOnInit() {
    this.loadItems();
  }

  public loadItems() {
    this.jasminService.getItems(this.itemsArrived.bind(this));
  }
  
  private itemsArrived(itemsResult: Observable<any>){
    this.items = itemsResult;
  }
}
