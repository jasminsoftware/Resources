import { Component, OnInit, Input, ViewChildren, QueryList  } from '@angular/core';
import { CustomerComponent } from '../customer/customer.component';
import { JasminService } from '../../services/jasmin.service';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent implements OnInit {
  @ViewChildren('war') customerComponents:QueryList<CustomerComponent>;

  private customers:any;

  constructor(private jasminService: JasminService) { }

  ngOnInit() {
    this.loadCustomers();
  }

  public loadCustomers() {
    this.jasminService.getCustomers(this.customersArrived.bind(this));
  }
  
  private customersArrived(customersResult: Observable<any>){
    this.customers = customersResult;
  }
}
