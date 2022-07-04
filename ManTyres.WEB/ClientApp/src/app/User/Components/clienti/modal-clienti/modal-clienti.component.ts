import { Component, OnInit, OnDestroy, Inject, HostListener } from '@angular/core';
import { Subscription } from 'rxjs';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Mode } from 'src/app/Shared/Models/mode.model';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Response } from 'src/app/Shared/Models/response.model'
import { ValidatorsService } from 'src/app/Shared/Validators/validators.services';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import Swal from 'sweetalert2';
import { Cliente } from '../../../../Shared/Models/clienti.model';
import { ClientiService } from '../../../Services/clienti.service';
import { ClientiComponent } from '../clienti/clienti.component';
import { VeicoliService } from '../../../Services/veicoli.service';
import { Veicolo } from '../../../../Shared/Models/veicoli.mdel';
import { ModalVeicoliComponent } from '../../veicoli/modal-veicoli/modal-veicoli.component';
import { UserService } from 'src/app/Auth/Services/user.service';

@Component({
  selector: 'app-modal-clienti',
  templateUrl: './modal-clienti.component.html',
  styleUrls: ['./modal-clienti.component.scss']
})
export class ModalClientiComponent implements OnInit, OnDestroy {
  subscribers: Subscription[];
  currentUserRole: string = '';

  privatoForm: FormGroup = this.formBuilder.group({});
  creationClienteControl: AbstractControl | undefined;
  get getPrivatoControl() { return this.privatoForm.controls; }

  aziendaForm: FormGroup = this.formBuilder.group({});
  creationAziendaControl: AbstractControl | undefined;
  get getAziendaControl() { return this.aziendaForm.controls; }

  cliente: Cliente | undefined;
  veicoli: Veicolo[] = [];
  mode: Mode | undefined;
  isAzienda: boolean = false;
  indice: number = 0;

  constructor(
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<ClientiComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public clientiService: ClientiService,
    private veicoliService: VeicoliService,
    private userService: UserService,
    private toastr: ToastrService,
    private validators: ValidatorsService,
  ) {
    this.subscribers = [];
  }

  ngOnInit(): void {
    this.mode = this.data.mode;
    if (this.mode == Mode.Edit) {
      if (this.data.cliente.isAzienda) {
        this.indice = 1;
        this.isAzienda = true;
      }
      else {
        this.indice = 0;
        this.isAzienda = false;
      }
    }

    this.privatoForm = this.formBuilder.group({
      nome: [this.data.cliente.nome, [Validators.required, Validators.maxLength(50), this.validators.fullName()]],
      codiceFiscale: [this.data.cliente.codiceFiscale, [this.validators.codiceFiscale()]],
      partitaIVA: [this.data.cliente.partitaIva, [Validators.minLength(11), Validators.maxLength(11)]],
      nazione: [this.data.cliente.nazione, [Validators.maxLength(100)]],
      provincia: [this.data.cliente.provincia, [Validators.maxLength(100)]],
      CAP: [this.data.cliente.cap, [Validators.min(10000), Validators.max(99999)]],
      comune: [this.data.cliente.comune, [Validators.maxLength(100)]],
      indirizzo: [this.data.cliente.indirizzo, [Validators.maxLength(200)]],
      email: [this.data.cliente.email, [Validators.email]],
      telefono: [this.data.cliente.telefono, [Validators.maxLength(20)]]
    });

    this.aziendaForm = this.formBuilder.group({
      nome: [this.data.cliente.nome, [Validators.required, Validators.maxLength(50), this.validators.fullName()]],
      partitaIVA: [this.data.cliente.partitaIva, [Validators.minLength(11), Validators.maxLength(11)]],
      nazione: [this.data.cliente.nazione, [Validators.maxLength(100)]],
      provincia: [this.data.cliente.provincia, [Validators.maxLength(100)]],
      CAP: [this.data.cliente.cap, [Validators.min(10000), Validators.max(99999)]],
      comune: [this.data.cliente.comune, [Validators.maxLength(100)]],
      indirizzo: [this.data.cliente.indirizzo, [Validators.maxLength(200)]],
      email: [this.data.cliente.email, [Validators.email]],
      telefono: [this.data.cliente.telefono, [Validators.maxLength(20)]]
    });

    this.subscribers.push(this.userService.userRole.subscribe(res => this.currentUserRole = res));
    if (this.IsViewMode())
      this.subscribers.push(this.veicoliService.getByclienteId(this.data.cliente.clienteId).subscribe(res => { console.log(res); this.veicoli = res.content; }));

  }

  EditMode() { this.mode = Mode.Edit; }
  ViewMode() { this.mode = Mode.View; }
  IsEditMode() { return this.mode === Mode.Edit; }
  IsEditOrNewMode() { return this.mode === Mode.Edit || this.mode === Mode.New; }
  IsNewMode() { return this.mode === Mode.New; }
  IsViewMode() { return this.mode === Mode.View; }

  ShowEdit() {
    this.mode = Mode.Edit;
    if (this.data.cliente.isAzienda) {
      this.indice = 1;
      this.isAzienda = true;
    }
    else {
      this.indice = 0;
      this.isAzienda = false;
    }
  }

  IsAzienda($event: any) {
    if ($event == 1) {
      this.isAzienda = true;
    }
    else {
      this.isAzienda = false;
    }
  }

  submitForm() {
    console.log('start');
    if (this.privatoForm.invalid && this.isAzienda == false) { return; }
    if (this.aziendaForm.invalid && this.isAzienda) { return; }
    console.log('first check');

    if (this.isAzienda) {
      this.cliente = {
        clienteId: this.data.cliente.clienteId,
        cognome: null,
        nome: this.replaceSpacesInOne(this.aziendaForm.value.nome.trim()),
        codiceFiscale: null,
        partitaIva: this.aziendaForm.value.partitaIVA,
        nazione: this.aziendaForm.value.nazione,
        provincia: this.aziendaForm.value.provincia,
        cap: this.aziendaForm.value.CAP,
        comune: this.aziendaForm.value.comune,
        indirizzo: this.aziendaForm.value.indirizzo,
        civico: null,
        email: this.aziendaForm.value.email,
        telefono: this.aziendaForm.value.telefono,
        isAzienda: this.isAzienda,
        isDeleted: false
      };
    }
    else {
      this.cliente = {
        clienteId: this.data.cliente.clienteId,
        cognome: null,
        nome: this.replaceSpacesInOne(this.privatoForm.value.nome.value.trim()),
        codiceFiscale: this.upperCase(this.privatoForm.value.codiceFiscale.value),
        partitaIva: null,
        nazione: this.privatoForm.value.nazione,
        provincia: this.privatoForm.value.provincia,
        cap: this.privatoForm.value.CAP,
        comune: this.privatoForm.value.comune,
        indirizzo: this.privatoForm.value.indirizzo,
        civico: null,
        email: this.privatoForm.value.email,
        telefono: this.privatoForm.value.telefono,
        isAzienda: this.isAzienda,
        isDeleted: false,
      };
    }

    if (this.IsNewMode()) {
      this.subscribers.push(this.clientiService.addCliente(this.cliente).subscribe(
        (res: Response) => {
          this.cliente = res.content;
          this.dialogRef.close(this.cliente);
          this.toastr.success('Cliente aggiunto con successo!');
        }
      ));
    }
    else {
      this.subscribers.push(this.clientiService.editCliente(this.cliente).subscribe(
        (res: Response) => {
          this.cliente = res.content;
          this.dialogRef.close(this.cliente);
          this.toastr.success('Cliente aggiornato con successo!');
        }
      ));
    }
  }

  closeDialog() {
    if (this.IsEditMode() || this.IsViewMode())
      this.dialogRef.close(this.cliente);
    else
      this.dialogRef.close(null);
  }


  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

  buttonDisabled() {
    if (
      this.IsEditMode()
      && !this.isAzienda
      && this.getPrivatoControl.nome.valid
      && this.getPrivatoControl.codiceFiscale.valid
      && this.getPrivatoControl.nazione.valid
      && this.privatoForm.value.provincia.value
      && this.privatoForm.value.CAP.value
      && this.privatoForm.value.comune.value
      && this.privatoForm.value.indirizzo.value
      && this.privatoForm.value.email.value
      && this.privatoForm.value.telefono.value
    )
      return false;
    if (
      this.IsEditMode()
      && this.isAzienda
      && this.getPrivatoControl.nome.valid
      && this.getPrivatoControl.codiceFiscale.valid
      && this.getPrivatoControl.nazione.valid
      && this.privatoForm.value.provincia.value
      && this.privatoForm.value.CAP.value
      && this.privatoForm.value.comune.value
      && this.privatoForm.value.indirizzo.value
      && this.privatoForm.value.email.value
      && this.privatoForm.value.telefono.value
    )
      return false;
    if (this.privatoForm.valid && !this.isAzienda)
      return false;
    if (this.aziendaForm.valid && this.isAzienda)
      return false;
    else
      return true;
  }

  replaceSpacesInOne(text: string) {
    if (!text) return text;
    return text.replace(/(?:\s+)\s/g, ' ');
  }

  upperCase(text: string) {
    if (!text) return text;
    return text.trim().toUpperCase();
  }

  getWidth(): number {
    return window.innerWidth;
  }

  deleteCliente(id: number) {
    Swal.fire({
      title: 'Confermare la cancellazione?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.value) {
        this.toastr.info('operazione in corso');
        this.subscribers.push(this.clientiService.deleteCliente(id).subscribe(
          () => {
            this.closeDialog();
            this.toastr.clear();
            this.toastr.success('Cliente eliminato con successo!');
          }
        ));
      }
    });
  }

  reactiveCliente(data: any) {
    Swal.fire({
      title: 'Ripristinare cliente?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    })
      .then((result) => {
        if (result.value) {
          data.IsDeleted = false;
          this.toastr.info('operazione in corso');
          this.subscribers.push(this.clientiService.editCliente(data).subscribe(
            () => {
              this.closeDialog();
              this.toastr.clear();
              this.toastr.success('Cliente riattivato con successo!');
            }
          ));
        }
      });
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Event) { this.getWidth(); }
}
