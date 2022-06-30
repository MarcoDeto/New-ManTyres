import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs/internal/Subscription';
import { Mode } from '../../../../Shared/Models/mode.model';
import { Sedi } from '../../../../Shared/Models/sedi.model';
import { SediService } from '../../../Services/sedi.service';

@Component({
  selector: 'app-sede-detail',
  templateUrl: './sede-detail.component.html',
  styleUrls: ['./sede-detail.component.scss']
})
export class SedeDetailComponent implements OnInit {
  title = 'Sede'
  sedeId = this.route.snapshot.paramMap.get('sedeId');
  subscribers: Subscription[] = [];
  sede: Sedi | undefined = undefined;
  mode: Mode = Mode.New;

  sedeForm  = this.formBuilder.group({
    nazione: [this.sede?.nazione, [Validators.required, Validators.maxLength(100)]],
    provincia: [this.sede?.provincia, [Validators.required, Validators.maxLength(100)]],
    cap: [this.sede?.cap, [Validators.required, Validators.minLength(5), Validators.maxLength(5)]],
    comune: [this.sede?.comune, [Validators.required, Validators.maxLength(100)]],
    indirizzo: [this.sede?.indirizzo, [Validators.required, Validators.maxLength(200)]],
    civico: [this.sede?.civico, [Validators.required, Validators.maxLength(10)]],
    telefono: [this.sede?.telefono, [Validators.required, Validators.maxLength(20)]],
    email: [this.sede?.email, [Validators.required, Validators.email]]
  });
  get getSedeControl() { return this.sedeForm.controls; }

  IsEditMode() { return this.mode === Mode.Edit; }
  IsNewMode() { return this.mode === Mode.New; }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private sediService: SediService,
    private toastr: ToastrService
  ) {  }

  ngOnInit(): void {
    if (this.sedeId != null && this.sedeId != 'Add') {
      this.mode = Mode.Edit;
      this.subscribers.push(this.sediService.getSedeById(this.sedeId).subscribe(
        res => {
          this.sede = res.content;
          this.sedeForm = this.formBuilder.group({
            sedeId: [this.sede?.sedeId, [Validators.required]],
            nazione: [this.sede?.nazione, [Validators.required, Validators.maxLength(100)]],
            provincia: [this.sede?.provincia, [Validators.required, Validators.maxLength(100)]],
            cap: [this.sede?.cap, [Validators.required, Validators.minLength(5), Validators.maxLength(5)]],
            comune: [this.sede?.comune, [Validators.required, Validators.maxLength(100)]],
            indirizzo: [this.sede?.indirizzo, [Validators.required, Validators.maxLength(200)]],
            civico: [this.sede?.civico, [Validators.required, Validators.maxLength(10)]],
            telefono: [this.sede?.telefono, [Validators.required, Validators.maxLength(20)]],
            email: [this.sede?.email, [Validators.required, Validators.email]]
          });
        }
      ));
    }
  }

  onSubmit() {
    if (this.IsEditMode()) {
      this.subscribers.push(this.sediService.editSede(this.sedeForm.value).subscribe(
        res => {
          this.toastr.success('Sede modificata con successo!');
          this.router.navigate(['/admin/sedi']);
        }
      ));
    }
    else {
      this.subscribers.push(this.sediService.addSede(this.sedeForm.value).subscribe(
        res => {
          this.toastr.success('Sede aggiunta con successo!');
          this.router.navigate(['/admin/sedi']);
        }
      ));
    }
  }

  Delete(sedeId: number) {
    this.subscribers.push(this.sediService.deactiveSede(sedeId).subscribe(
      res => {
        this.toastr.success('Sede eliminata con successo!');
        this.router.navigate(['/admin/sedi']);
      }
    ));
  }

  getWidth(): number {
    return window.innerWidth;
  }
}
