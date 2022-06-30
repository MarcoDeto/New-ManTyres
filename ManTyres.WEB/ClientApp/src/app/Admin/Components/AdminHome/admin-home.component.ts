import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrls: ['./admin-home.component.scss']
})
export class AdminHomeComponent implements OnInit, OnDestroy {
  title = 'Home';

  currentUserName: string = '';

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {

  }

  ngOnDestroy(): void { }

}
