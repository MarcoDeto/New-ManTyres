import { Component, DoCheck, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AnyCatcher } from 'rxjs/internal/AnyCatcher';
import { Place } from 'src/app/Shared/Models/place.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-place',
  templateUrl: './place.component.html',
  styleUrls: ['./place.component.scss']
})

export class PlaceComponent implements OnInit, DoCheck, OnDestroy {
  @Input() public place: Place | null = null;
  GOOGLE_MAPS_API_KEY = environment.GOOGLE_MAPS_API_KEY;
  imageWidth: number = 0;
  @ViewChild('image') image: any;
  imageSelected: number = 0;
  @ViewChild('carousel') carousel: any;


  constructor() { }

  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

  ngOnInit() {
    
  }

  ngDoCheck(): void {
    console.log(this.carousel);
    if (this.image && this.image.nativeElement && this.imageWidth != this.image.nativeElement.width) {
      this.getImageWidth();
    }
  }

  getImageWidth() {
    this.imageWidth = this.image.nativeElement.width;
  }

  previous() {
    if (this.imageSelected == 0) {
      this.imageSelected = this.place!.google_Photos.length - 1;
    } else {
      this.imageSelected = this.imageSelected - 1;
    }
  }

  next() {
    if (this.imageSelected == this.place!.google_Photos.length - 1) {
      this.imageSelected = 0;
    } else {
      this.imageSelected = this.imageSelected + 1;
    }
  }
}
