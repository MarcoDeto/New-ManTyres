import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { CardPneumaticiComponent } from '../../../../Shared/Components/card-pneumatici/card-pneumatici.component';
import { Inventario } from '../../../../Shared/Models/inventario.model';
import { Mode } from '../../../../Shared/Models/mode.model';
import { UserService } from '../../../../Shared/Services/user.service';
import { PneumaticiService } from '../../../Services/PneumaticiService';
import { Response } from 'src/app/Shared/Models/response.model';
import { Component, Inject, OnInit } from '@angular/core';
import { Sedi } from '../../../../Shared/Models/sedi.model';
import { SediService } from '../../../../Admin/Services/sedi.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-inizio-deposito',
  templateUrl: './inizio-deposito.component.html',
  styleUrls: ['./inizio-deposito.component.scss']
})

export class InizioDepositoComponent implements OnInit {
  title = "Fine deposito";
  subscribers: Subscription[] = [];
  mode: Mode = Mode.Edit;
  inventario: Inventario | undefined = undefined;
  currentUserid = "";
  sedi: Sedi[] = [];

  inizioDepositoForm = this.formBuilder.group({
    sede: [this.data?.inventario.deposito.sedeId, [Validators.required]],
    ubicazione: [this.data?.inventario.deposito.ubicazione, [Validators.required, Validators.maxLength(250)]],
    inizioDeposito: [moment(new Date()), [Validators.required]],
    battistrada: [this.data?.inventario.battistrada, [Validators.required, Validators.maxLength(3)]],
    statoGomme: [this.data?.inventario.statoGomme, [Validators.required]]
  })

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<CardPneumaticiComponent>,
    public dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private toastr: ToastrService,
    private userService: UserService,
    private pneumaticiService: PneumaticiService,
    private sediService: SediService,
  ) { }

  EditMode() { this.mode = Mode.Edit; }
  ViewMode() { this.mode = Mode.View; }
  IsEditMode() { return this.mode === Mode.Edit; }
  IsEditOrNewMode() { return this.mode === Mode.Edit || this.mode === Mode.New; }
  IsNewMode() { return this.mode === Mode.New; }
  IsViewMode() { return this.mode === Mode.View; }

  ngOnInit(): void {
    this.mode = this.data.mode;
    this.inventario = this.data.inventario;
    this.getSedi();
    this.currentUserid = this.userService.getUserID();
  }

  getSedi(): void {
    this.subscribers.push(this.sediService.getAllSedi().subscribe(
      (res: Response) => {
        if (res) {
          this.sedi = res.content;
        }
      },
      err => {
        this.sedi.length = 0;
        this.sedi.splice(0);
      }
    ));
  }

  submitForm() {
    this.inventario.userId = this.currentUserid;
    this.inventario.deposito.sedeId = this.inizioDepositoForm.get('sede').value;
    this.inventario.deposito.ubicazione = this.inizioDepositoForm.get('ubicazione').value;
    this.inventario.inizioDeposito = this.inizioDepositoForm.get('inizioDeposito').value;
    console.log("inizio deposito", this.inventario);

    this.subscribers.push(this.pneumaticiService.inizioDeposito(this.inventario).subscribe(
      (res: Response) => {
        this.dialogRef.close(this.data.inventario);
        this.toastr.success('Deposito aggiornato con successo!');
        Swal.fire({
          title: 'Salvare PDF?',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonText: 'Sì',
          cancelButtonText: 'No'
        }).then((result) => {
          if (result.isConfirmed) {
            console.log("PDF");
            this.pneumaticiService.generatePdf(res.content);
          }
        });
      }
    ));
  }

  closeDialog() {
    this.dialogRef.close(null);
  }
}
