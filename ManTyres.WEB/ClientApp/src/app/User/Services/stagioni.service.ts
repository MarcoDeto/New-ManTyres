import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
import { Response } from 'src/app/Shared/Models/response.model';

@Injectable({
  providedIn: 'root'
})
export class StagioniService {

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  getAllStagioni(): Observable<Response> {
    return this.http.get<Response>(environment.stagioni + '/GetAll');
  }
}
