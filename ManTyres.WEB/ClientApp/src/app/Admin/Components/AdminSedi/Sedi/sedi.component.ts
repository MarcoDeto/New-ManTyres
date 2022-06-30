import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SediService } from '../../../Services/sedi.service';
import { Response } from 'src/app/Shared/Models/response.model';
import { Sedi } from '../../../../Shared/Models/sedi.model';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-sedi',
  templateUrl: './sedi.component.html',
  styleUrls: ['./sedi.component.scss']
})
export class SediComponent implements OnInit {
  title = 'Sedi';
  subscribers: Subscription[] = [];
  sedi: Sedi[] = [];
  hover = false;

  constructor(
    private route: ActivatedRoute,
    private sediService: SediService
  ) { }

  ngOnInit(): void {
    this.getSedi();
  }

  hoverListItem(hover: boolean) {
    this.hover = !this.hover;
  }

  getSedi() {
    this.subscribers.push(this.sediService.getAllSedi().subscribe(
      (res: Response) => {
        if (res) {
          this.sedi = res.content;
        }
      }
    ));
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }
}
