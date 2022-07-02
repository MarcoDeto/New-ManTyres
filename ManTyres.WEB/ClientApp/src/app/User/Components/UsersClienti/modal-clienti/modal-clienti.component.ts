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
import { ClientiService } from '../../../Services/ClientiService';
import { ClientiComponent } from '../clienti/clienti.component';
import { VeicoliService } from '../../../Services/VeicoliService';
import { Veicolo } from '../../../../Shared/Models/veicoli.mdel';
import { ModalVeicoliComponent } from '../../UsersVeicoli/modal-veicoli/modal-veicoli.component';
import { UserService } from 'src/app/Auth/Services/user.service';

@Component({
  selector: 'app-modal-clienti',
  templateUrl: './modal-clienti.component.html',
  styleUrls: ['./modal-clienti.component.scss']
})
export class ModalClientiComponent implements OnInit, OnDestroy {
  subscribers: Subscription[];
  currentUserRole: string = '';

  PrivatoForm: FormGroup = this.formBuilder.group({});
  creationClienteControl: AbstractControl | undefined;

  AziendaForm: FormGroup = this.formBuilder.group({});
  creationAziendaControl: AbstractControl | undefined;

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

    this.PrivatoForm = this.formBuilder.group({
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

    this.AziendaForm = this.formBuilder.group({
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
    console.log("start");
    if (this.PrivatoForm.invalid && this.isAzienda == false) { return; }
    if (this.AziendaForm.invalid && this.isAzienda) { return; }
    console.log("first check");

    if (this.isAzienda) {
      this.cliente = {
        clienteId: this.data.cliente.clienteId,
        cognome: null,
        nome: this.replaceSpacesInOne(this.AziendaForm.get('nome').value.trim()),
        codiceFiscale: null,
        partitaIva: this.AziendaForm.get('partitaIVA').value,
        nazione: this.AziendaForm.get('nazione').value,
        provincia: this.AziendaForm.get('provincia').value,
        cap: this.AziendaForm.get('CAP').value,
        comune: this.AziendaForm.get('comune').value,
        indirizzo: this.AziendaForm.get('indirizzo').value,
        civico: null,
        email: this.AziendaForm.get('email').value,
        telefono: this.AziendaForm.get('telefono').value,
        isAzienda: this.isAzienda,
        isDeleted: false,
        dataCreazione: null,
      };
    }
    else {
      this.cliente = {
        clienteId: this.data.cliente.clienteId,
        cognome: null,
        nome: this.replaceSpacesInOne(this.PrivatoForm.get('nome').value.trim()),
        codiceFiscale: this.upperCase(this.PrivatoForm.get('codiceFiscale').value),
        partitaIva: null,
        nazione: this.PrivatoForm.get('nazione').value,
        provincia: this.PrivatoForm.get('provincia').value,
        cap: this.PrivatoForm.get('CAP').value,
        comune: this.PrivatoForm.get('comune').value,
        indirizzo: this.PrivatoForm.get('indirizzo').value,
        civico: null,
        email: this.PrivatoForm.get('email').value,
        telefono: this.PrivatoForm.get('telefono').value,
        isAzienda: this.isAzienda,
        isDeleted: false,
        dataCreazione: null,
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
      && this.PrivatoForm.get('nome').valid
      && this.PrivatoForm.get('codiceFiscale').valid
      && this.PrivatoForm.get('nazione').valid
      && this.PrivatoForm.get('provincia').value
      && this.PrivatoForm.get('CAP').value
      && this.PrivatoForm.get('comune').value
      && this.PrivatoForm.get('indirizzo').value
      && this.PrivatoForm.get('email').value
      && this.PrivatoForm.get('telefono').value
    )
      return false;
    if (
      this.IsEditMode()
      && this.isAzienda
      && this.PrivatoForm.get('nome').valid
      && this.PrivatoForm.get('partitaIVA').valid
      && this.PrivatoForm.get('nazione').valid
      && this.PrivatoForm.get('provincia').value
      && this.PrivatoForm.get('CAP').value
      && this.PrivatoForm.get('comune').value
      && this.PrivatoForm.get('indirizzo').value
      && this.PrivatoForm.get('email').value
      && this.PrivatoForm.get('telefono').value
    )
      return false;
    if (this.PrivatoForm.valid && !this.isAzienda)
      return false;
    if (this.AziendaForm.valid && this.isAzienda)
      return false;
    else
      return true;
  }

  replaceSpacesInOne(text: string) {
    if (!text) return text;
    return text.replace(/(?:\s+)\s/g, " ");
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
  onResize(event) {
    this.getWidth();
  }

}
