import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Inventario } from '../../../../Shared/Models/inventario.model';
import { Mode } from '../../../../Shared/Models/mode.model';
import { PneumaticiService } from '../../../Services/pneumatici.service';
import { Response } from 'src/app/Shared/Models/response.model';
import Swal from 'sweetalert2';
import { UserService } from 'src/app/Auth/Services/user.service';
import { CardPneumaticiComponent } from '../card-pneumatici/card-pneumatici.component';

@Component({
  selector: 'app-fine-deposito',
  templateUrl: './fine-deposito.component.html',
  styleUrls: ['./fine-deposito.component.scss']
})
export class FineDepositoComponent implements OnInit {
  title = 'Fine deposito';
  subscribers: Subscription[] = [];
  mode: Mode = Mode.Edit;
  inventario: Inventario = {
    pneumaticiId: 0,
    inizioDeposito: null,
    fineDeposito: null,
    depositoId: null,
    battistrada: null,
    statoGomme: null,
    userId: null,
    isDeleted: null,
    pneumatici: null,
    deposito: null,
    user: null
  }
  currentUserid: string | null = null;


  fineDepositoForm = this.formBuilder.group({
    fineDeposito: [moment(new Date()), [Validators.required]],
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
    this.currentUserid = this.userService.getUserID();
  }

  submitForm() {
    this.inventario.userId = this.currentUserid;
    this.inventario.fineDeposito = this.fineDepositoForm.value.fineDeposito;
    console.log(this.inventario);
    this.subscribers.push(this.pneumaticiService.fineDeposito(this.inventario).subscribe(
      (res: Response) => {
        console.log(res);
        this.dialogRef.close(null);
        this.toastr.success('Deposito aggiornato con successo!');
        Swal.fire({
          title: 'Salvare PDF?',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonText: 'S??',
          cancelButtonText: 'No'
        }).then((result) => {
          if (result.isConfirmed) {
            console.log('PDF');
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
