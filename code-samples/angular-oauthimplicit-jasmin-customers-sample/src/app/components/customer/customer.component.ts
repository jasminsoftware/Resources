import { Component, OnInit, Input } from '@angular/core';
import { CustomersComponent } from '../customers/customers.component';
import { JasminService } from '../../services/jasmin.service';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit {
  @Input() customerData:any;

  private customerDetails:any;

  constructor(private jasminService: JasminService) { }

  ngOnInit() {
  }

  private openCustomer(){
    this.getCustomerDetails();
  }

  private getCustomerDetails(){
    this.jasminService.getCustomerDetails(this.customerData.customerKey, this.customerDetailsArrived.bind(this));
  }

  private customerDetailsArrived(customerDetailsResult: Observable<any>){
    console.log(customerDetailsResult[0]);
    this.customerDetails = customerDetailsResult;
  }
}
