import { Component, Input, OnInit } from '@angular/core';
import { Sedi } from '../../../../Shared/Models/sedi.model';

@Component({
  selector: 'card-sede',
  templateUrl: './card-sede.component.html',
  styleUrls: ['./card-sede.component.scss']
})
export class CardSedeComponent implements OnInit {
  @Input()
  sede: Sedi | null = null;
  hover = false;

  constructor() { }

  ngOnInit(): void {
    console.log(this.sede);
  }

  hoverListItem(hover: boolean) {
    this.hover = !this.hover;
  }
}
