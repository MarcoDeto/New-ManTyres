import { HttpClient } from '@angular/common/http';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Mode } from '../../../Shared/Models/mode.model';
import { Pneumatici } from '../../../Shared/Models/pneumatici.model';
import { PneumaticiService } from '../../Services/pneumatici.service';
import { ModalPneumaticiComponent } from '../pneumatici/modal-pneumatici/modal-pneumatici.component';
import { Response } from 'src/app/Shared/Models/response.model';
import { Sedi } from '../../../Shared/Models/sedi.model';
import { Depositi } from '../../../Shared/Models/depositi.model';
import { Inventario } from '../../../Shared/Models/inventario.model';
import { FormBuilder, FormGroup } from '@angular/forms';
import Swal from 'sweetalert2';
import { FineDepositoComponent } from '../pneumatici/fine-deposito/fine-deposito.component';
import { InizioDepositoComponent } from '../pneumatici/inizio-deposito/inizio-deposito.component';


@Component({
  selector: 'app-user-home',
  templateUrl: './user-home.component.html',
  styleUrls: ['./user-home.component.scss']
})
export class UserHomeComponent implements OnInit, OnDestroy {
  title = 'Pneumatici';
  hover = false; caricamento = false;

  pneumaticiForm: FormGroup = this.formBuilder.group({
    targa: null,
  });

  inventario: Inventario[] = [];
  pneumatici: Pneumatici = {
    pneumaticiId: 0,
    marca: null,
    modello: null,
    misura: null,
    dot: '0000',
    stagioneId: null,
    veicoloId: null,
    dataUbicazione: null,
    quantita: 4,
    isDeleted: false,
    stagione: null,
    veicolo: null
  }
  sede: Sedi = {
    cap: null,
    civico: null,
    comune: null,
    isDeleted: false,
    nazione: null,
    provincia: null,
    sedeId: 0,
    telefono: null,
    indirizzo: null,
    email: null
  }
  deposito: Depositi = {
    depositoId: 0,
    sedeId: null,
    ubicazione: null,
    isDeleted: false,
    sede: this.sede
  }
  subscribers: Subscription[];

  constructor(
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private http: HttpClient,
    public dialog: MatDialog,
    private pneumaticiService: PneumaticiService,
    private toastr: ToastrService,
    private formBuilder: FormBuilder,

  ) {
    this.subscribers = [];
  }

  ngOnInit() {
    this.cercaPneumatici();
  }

  hoverListItem(hover: boolean) {
    this.hover = !this.hover;
  }

  getLastPneumatici() {
    this.caricamento = true;
    this.subscribers.push(this.pneumaticiService.getLast().subscribe({
      next: (res: Response) => {
        if (res) {
          this.inventario = res.content;
          this.caricamento = false;
        }
      },
      error: (error: any) => {
        this.caricamento = false;
        this.inventario.length = 0;
        this.inventario.splice(0);
      }
    }));
  }

  cercaPneumatici() {
    this.caricamento = true;
    this.subscribers.push(
      this.pneumaticiService.getPneumatici(this.pneumaticiForm.value.targa).subscribe({
      next: (res: Response) => {
        if (res) {
          this.inventario = res.content;
          this.caricamento = false;
        }
      },
      error: (error: any) => {
        this.inventario.length = 0;
        this.inventario.splice(0);
        this.caricamento = false;
      }})
    );
  }

  addPneumatici() {
    const dialogRef = this.dialog.open(ModalPneumaticiComponent, {
      maxWidth: '90%',
      data: { mode: Mode.New, inventario: new Inventario(0, '', '', 0, 0, '', '', false, this.pneumatici, this.deposito, null) }
    });

    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
  }

  editPneumatici(data: Inventario) {
    let response = '';
    if (data.fineDeposito == null) { response = 'Fine Deposito'; }
    else { response = 'Inizio Deposito'; }
    Swal.fire({
      icon: 'question',
      title: 'Cosa vuoi fare?',
      showDenyButton: true,
      denyButtonText: response,
      confirmButtonText: 'Modifica'
    }).then((result) => {
      if (result.isConfirmed) {
        const dialogRef = this.dialog.open(ModalPneumaticiComponent, {
          maxWidth: '90%',
          data: { mode: Mode.Edit, inventario: data }
        });
        dialogRef.afterClosed().subscribe(result => {
          if (result != undefined || result != null) {
            this.inventario = result;
          }
          this.cercaPneumatici();
        });
      }
      if (result.isDenied) {
        if (data.fineDeposito == null) {
          const dialogRef = this.dialog.open(FineDepositoComponent, {
            maxWidth: '90%',
            data: { mode: Mode.Edit, inventario: data }
          });
          dialogRef.afterClosed().subscribe(result => {
            this.cercaPneumatici();
          });
        }
        else {
          const dialogRef = this.dialog.open(InizioDepositoComponent, {
            maxWidth: '90%',
            data: { mode: Mode.Edit, inventario: data }
          });
          dialogRef.afterClosed().subscribe(result => {
            this.cercaPneumatici();
          });
        }
      }
    });
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }
}
