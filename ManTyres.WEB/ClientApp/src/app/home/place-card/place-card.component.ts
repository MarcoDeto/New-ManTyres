import { Component, DoCheck, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/Auth/Services/user.service';
import { Place } from 'src/app/Shared/Models/place.model';
import { PlaceService } from 'src/app/Shared/Services/place.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'place-card',
  templateUrl: './place-card.component.html',
  styleUrls: ['./place-card.component.scss']
})

export class PlaceCardComponent implements OnInit, DoCheck, OnDestroy {
  @Input() public place: Place | undefined;
  GOOGLE_MAPS_API_KEY = environment.GOOGLE_MAPS_API_KEY;
  imageWidth: number = 200;
  @ViewChild('image') image: any;
  imageSelected: number = 0;
  @ViewChild('carousel') carousel: any;

  constructor(
    public placesService: PlaceService,
    private userService: UserService,
    private router: Router,
  ) { }

  ngOnDestroy(): void {
    this.imageWidth = 0;
    this.imageSelected = 0;
  }

  ngOnInit() {

  }

  ngDoCheck(): void {
    if (this.image) {
      if (this.image.nativeElement) {
        if (this.image.nativeElement.width) {
          console.log('width');
          this.getImageWidth();
        }
      }
    }
  }

  getImageWidth() {
    if (this.image.nativeElement.width == 0) { return; }
    this.imageWidth = this.image.nativeElement.width;
    console.log('imageWidth = ' + this.imageWidth);
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

  favorite() {
    console.log(this.userService.isLoggedIn());
    if (this.userService.isLoggedIn() == false) {
      this.router.navigate(['/account/login']);
    }
  }

  getAddressLabel(address: string): string {
    var splitted = address.split(',');
    if (!splitted) { return ''; }
    else if (splitted.length > 1 ) {
      return splitted[0] +','+ splitted[1];
    }
    return splitted[0];
  }
  getCityLabel(address: string): string {
    var splitted = address.split(',');
    if (!splitted) { return ''; }
    else if (splitted.length > 2 ) {
      return splitted[2] + ',' + splitted[3];
    }
    return '';
  }

  openDetail() {
    this.placesService.placeSelected = this.place;
    window.open('place/'+this.place?.id, "_blank");
    //this.router.navigate(['/place/'+this.place?.id]);
  }
}
