import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Place } from 'src/app/Shared/Models/place.model';
import { PlaceService } from 'src/app/Shared/Services/place.service';
import { environment } from 'src/environments/environment';
import { Response } from '../../Shared/Models/response.model';

@Component({
  selector: 'app-place-detail',
  templateUrl: './place-detail.component.html',
  styleUrls: ['./place-detail.component.scss']
})
export class PlaceDetailComponent implements OnInit {
  GOOGLE_MAPS_API_KEY = environment.GOOGLE_MAPS_API_KEY;
  id = this.route.snapshot.paramMap.get('id');
  place : Place | undefined;
  constructor(
    public placesService: PlaceService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    if (this.placesService.placeSelected) {
      this.place = this.placesService.placeSelected;
    }
    else if (this.id) {
      this.placesService.GetById(this.id).subscribe(
        (res: Response) => {
          this.place = res.content;
        }
      );
    }
  }

}
