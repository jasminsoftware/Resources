import { Component, OnInit, Input } from '@angular/core';
import { JasminService } from '../../services/jasmin.service';
import { SettingsService } from '../../services/settings.service';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit {
  @Input() itemData: any;

  private flip:boolean;

  constructor(private jasminService: JasminService, private settingsService: SettingsService) { }

  ngOnInit() {
  }

  private toggleFlip() {
    this.flip = !this.flip;
  }
}
