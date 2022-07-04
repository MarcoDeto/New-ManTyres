import { Component, OnInit, OnDestroy, Inject, HostListener } from '@angular/core';
import { ReplaySubject, Subject, Subscription } from 'rxjs';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Mode } from 'src/app/Shared/Models/mode.model';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Response } from 'src/app/Shared/Models/response.model'
import { ValidatorsService } from 'src/app/Shared/Validators/validators.services';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import Swal from 'sweetalert2';
import { Veicolo } from '../../../../Shared/Models/veicoli.mdel';
import { VeicoliComponent } from '../veicoli/veicoli.component';
import { VeicoliService } from '../../../Services/veicoli.service';
import { Cliente } from '../../../../Shared/Models/clienti.model';
import { takeUntil } from 'rxjs/operators';
import { ClientiService } from '../../../Services/clienti.service';
import { ModalClientiComponent } from '../../clienti/modal-clienti/modal-clienti.component';
import { PneumaticiService } from '../../../Services/pneumatici.service';
import { Inventario } from '../../../../Shared/Models/inventario.model';
import { ModalPneumaticiComponent } from '../../pneumatici/modal-pneumatici/modal-pneumatici.component';
import { FineDepositoComponent } from '../../pneumatici/fine-deposito/fine-deposito.component';
import { InizioDepositoComponent } from '../../pneumatici/inizio-deposito/inizio-deposito.component';
import { UserService } from 'src/app/Auth/Services/user.service';

@Component({
  selector: 'app-modal-veicoli',
  templateUrl: './modal-veicoli.component.html',
  styleUrls: ['./modal-veicoli.component.scss']
})

export class ModalVeicoliComponent implements OnInit, OnDestroy {
  subscribers: Subscription[];
  currentUserRole: string | null = null;

  veicoloForm: FormGroup = this.formBuilder.group({});

  veicolo: Veicolo | undefined;
  mode: Mode | undefined;

  inventario: Inventario[] = [];

  clienti: Cliente[] = [];
  /** control for filter for server side. */
  public filter: FormControl = new FormControl();
  /** list of clienti filtered after simulating server side search */
  public filtered: ReplaySubject<Cliente[]> = new ReplaySubject<Cliente[]>(1);
  /** Subject that emits when the component has been destroyed. */
  protected _onDestroy = new Subject<void>();

  constructor(
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<VeicoliComponent>,
    public dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private veicoliService: VeicoliService,
    private clientiService: ClientiService,
    private pneumaticiService: PneumaticiService,
    private userService: UserService,
    private toastr: ToastrService,
    private validators: ValidatorsService
  ) {
    this.subscribers = [];
  }

  ngOnInit(): void {
    this.getClienti();

    this.mode = this.data.mode;

    this.veicoloForm = this.formBuilder.group({
      cliente: [this.data.veicolo.clienteId, [Validators.required]],
      targa: [this.data.veicolo.targa, [Validators.required, Validators.maxLength(10)]],
      marca: [this.data.veicolo.marca, [Validators.maxLength(50)]],
      modello: [this.data.veicolo.modello, [Validators.maxLength(50)]]
    });

    // load the initial list
    this.filtered.next(this.clienti.slice());
    // listen for search field value changes
    this.filter.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => { this.filtering(); })

    this.subscribers.push(this.userService.userRole.subscribe(res => this.currentUserRole = res));

    if (this.IsViewMode())
      this.subscribers.push(this.pneumaticiService.getPneumatici(this.data.veicolo.targa).subscribe(res => { console.log(res); this.inventario = res.content; }));
  }

  protected filtering() {
    if (this.clienti.length == 0) { return; }
    // get the search keyword
    let search = this.filter.value;
    if (!search) {
      this.filtered.next(this.clienti.slice());
      return;
    }
    else { search = search.toLowerCase(); }
    // filtering
    this.filtered.next(
      this.clienti.filter(type => type.nome && type.nome.toLowerCase().indexOf(search) > -1)
    );
  }

  addCliente() {
    const dialogRef = this.dialog.open(ModalClientiComponent, {
      maxWidth: '90%', width: '90%',
      data: { mode: Mode.New, cliente: new Cliente(0, null, null, null, null, null, null, null, null, null, null, null, null, false, false) }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.ngOnInit();
      if (result != null)
        this.data.veicolo.clienteId = result.clienteId;
    });
  }

  EditMode() { this.mode = Mode.Edit; }
  ViewMode() { this.mode = Mode.View; }
  IsEditMode() { return this.mode === Mode.Edit; }
  IsEditOrNewMode() { return this.mode === Mode.Edit || this.mode === Mode.New; }
  IsNewMode() { return this.mode === Mode.New; }
  IsViewMode() { return this.mode === Mode.View; }

  getClienti(): void {
    this.subscribers.push(this.clientiService.getAllClienti().subscribe(
      (res: Response) => {
        this.clienti = res.content;
        this.filtered.next(this.clienti);
      },
      err => {
        this.clienti.length = 0;
        this.clienti.splice(0);
      }
    ));
  }

  isAdmin() { return this.currentUserRole === 'Admin' }

  submitForm() {
    if (this.IsEditOrNewMode() == false || this.veicoloForm.invalid == true) { return; }
    console.log('EDIT');
    this.veicolo = {
      veicoloId: this.data.veicolo.veicoloId,
      targa: this.upperCase(this.veicoloForm.value.targa),
      marca: this.replaceSpacesInOne(this.veicoloForm.value.marca),
      modello: this.replaceSpacesInOne(this.veicoloForm.value.modello),
      clienteId: this.veicoloForm.value.cliente,
      isDeleted: false,
      cliente: null
    };

    if (this.IsNewMode()) {
      this.subscribers.push(this.veicoliService.addVeicolo(this.veicolo).subscribe(
        (res: Response) => {
          this.veicolo = res.content;
          this.dialogRef.close(this.veicolo);
          this.toastr.success('Veicolo aggiunto con successo!');
        }
      ));
    }
    else {
      this.subscribers.push(this.veicoliService.editVeicolo(this.veicolo).subscribe(
        (res: Response) => {
          this.veicolo = res.content;
          this.dialogRef.close(this.veicolo);
          this.toastr.success('Veicolo aggiornato con successo!');
        }
      ));
    }
  }

  closeDialog() {
    if (this.IsEditMode() || this.IsViewMode())
      this.dialogRef.close(this.veicolo);
    else
      this.dialogRef.close(null);
  }


  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

  replaceSpacesInOne(text: string) {
    if (!text) return text;
    text = text.charAt(0).toUpperCase() + text.substr(1).toLowerCase();
    return text.trim().replace(/(?:\s+)\s/g, ' ');
  }

  upperCase(text: string) {
    if (!text) return text;
    return text.trim().toUpperCase();
  }

  getWidth(): number {
    return window.innerWidth;
  }

  deactivateVeicolo(id: number) {
    Swal.fire({
      title: 'Confermare la cancellazione?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    })
      .then((result) => {
        if (result.value) {
          this.subscribers.push(this.veicoliService.deactivateVeicolo(id).subscribe(
            () => {
              this.ngOnInit();
              this.toastr.success('Veicolo eliminato con successo!');
            }
          ));
        }
      });
  }

  reactivateVeicolo(id: number) {
    Swal.fire({
      title: 'Ripristinare veicolo?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    })
      .then((result) => {
        if (result.value) {
          this.subscribers.push(this.veicoliService.reactivateVeicolo(id).subscribe(
            () => {
              this.ngOnInit();
              this.toastr.success('Veicolo riattivato con successo!');
            }
          ));
        }
      });
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
          this.closeDialog();
        });
      }
      if (result.isDenied) {
        if (data.fineDeposito == null) {
          const dialogRef = this.dialog.open(FineDepositoComponent, {
            maxWidth: '90%',
            data: { mode: Mode.Edit, inventario: data }
          });
          dialogRef.afterClosed().subscribe(result => {
            this.closeDialog();
          });
        }
        else {
          const dialogRef = this.dialog.open(InizioDepositoComponent, {
            maxWidth: '90%',
            data: { mode: Mode.Edit, inventario: data }
          });
          dialogRef.afterClosed().subscribe(result => {
            this.closeDialog();
          });
        }
      }
    });
  }

  removeCliente() {
    this.veicoloForm.get('cliente')?.setValue(0);
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Event) { this.getWidth(); }

}
