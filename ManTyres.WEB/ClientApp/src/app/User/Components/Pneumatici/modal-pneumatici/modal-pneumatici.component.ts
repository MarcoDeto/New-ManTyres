import { Component, HostListener, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl, FormControl, Form } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Response } from 'src/app/Shared/Models/response.model';
import { ReplaySubject, Subject, Subscription } from 'rxjs';
import { debounceTime, delay, filter, map, takeUntil, tap } from 'rxjs/operators';
import { Mode } from '../../../../Shared/Models/mode.model';
import { Pneumatici } from '../../../../Shared/Models/pneumatici.model';
import { Veicolo } from '../../../../Shared/Models/veicoli.mdel';
import { ValidatorsService } from '../../../../Shared/Validators/validators.services';
import { VeicoliService } from '../../../Services/veicoli.service';
import { ModalVeicoliComponent } from '../../veicoli/modal-veicoli/modal-veicoli.component';
import { UserHomeComponent } from '../../home/user-home.component';
import { PneumaticiService } from '../../../Services/pneumatici.service';
import Swal from 'sweetalert2';
import { StagioniService } from '../../../Services/stagioni.service';
import { Sedi } from '../../../../Shared/Models/sedi.model';
import { Stagioni } from '../../../../Shared/Models/stagioni.model';
import { Depositi } from '../../../../Shared/Models/depositi.model';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as moment from 'moment';
import { SediService } from '../../../../Admin/Services/sedi.service';
import { Inventario } from '../../../../Shared/Models/inventario.model';
import { ThemePalette } from '@angular/material/core';
import { UserService } from 'src/app/Auth/Services/user.service';

@Component({
  selector: 'app-modal-pneumatici',
  templateUrl: './modal-pneumatici.component.html',
  styleUrls: ['./modal-pneumatici.component.scss']
})
export class ModalPneumaticiComponent implements OnInit, OnDestroy {
  @ViewChild('bigPdfViewer', { static: true }) public bigPdfViewer: any;
  @ViewChild('picker') picker: any;

  public date = new Date();
  public showSpinners = true;
  public showSeconds = false;
  public touchUi = false;
  public enableMeridian = false;
  public minDate = new Date();
  public maxDate = new Date();
  public stepHour = 1;
  public stepMinute = 1;
  public stepSecond = 1;
  public color: ThemePalette = 'primary';

  subscribers: Subscription[] = [];
  currentUserid: string | null = null;
  currentUserRole: string | null = null;

  pneumaticiForm: FormGroup = this.formBuilder.group({});

  creationClientiControl: AbstractControl | undefined;

  pneumatici: Pneumatici | undefined;
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
    sedeId: 0,
    ubicazione: null,
    isDeleted: false,
    sede: this.sede
  }
  sedi: Sedi[] = [];
  stagioni: Stagioni[] = [];
  veicoli: Veicolo[] = [];
  mode: Mode = Mode.New;

  inventario: Inventario | undefined = undefined;

  /** control for filter for server side. */
  public filter: FormControl = new FormControl();
  /** list of clienti filtered after simulating server side search */
  public filtered: ReplaySubject<Veicolo[]> = new ReplaySubject<Veicolo[]>(1);
  /** Subject that emits when the component has been destroyed. */
  protected _onDestroy = new Subject<void>();

  DATE_TIME_FORMAT = 'DD-MM-yyyy hh:mm:ss';
  hiddenPopUpPdf: Boolean = false;

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<UserHomeComponent>,
    public dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private veicoliService: VeicoliService,
    private pneumaticiService: PneumaticiService,
    private sediService: SediService,
    private stagioniService: StagioniService,
    private userService: UserService,
    private toastr: ToastrService,
    private validators: ValidatorsService
  ) { }

  EditMode() { this.mode = Mode.Edit; }
  ViewMode() { this.mode = Mode.View; }
  IsEditMode() { return this.mode === Mode.Edit; }
  IsEditOrNewMode() { return this.mode === Mode.Edit || this.mode === Mode.New; }
  IsNewMode() { return this.mode === Mode.New; }
  IsViewMode() { return this.mode === Mode.View; }

  ngOnInit(): void {
    this.getStagioni();
    this.getVeicoli();
    this.getSedi();
    this.mode = this.data.mode;
    this.hiddenPopUpPdf = this.userService.getHiddenPopUpPdf() == 'true' ? true : false;

    this.pneumaticiForm = this.formBuilder.group({
      stagioneId: [this.data?.inventario.pneumatici.stagioneId, [Validators.required]],
      marca: [this.data?.inventario.pneumatici.marca, [Validators.required, Validators.maxLength(50)]],
      modello: [this.data?.inventario.pneumatici.modello, [Validators.maxLength(50)]],
      misura: [this.data?.inventario.pneumatici.misura, [Validators.required, Validators.maxLength(20)]],
      battistrada: [this.data?.inventario.battistrada, [Validators.required, Validators.min(0)]],
      dot: [this.data?.inventario.pneumatici.dot, [Validators.minLength(4), Validators.maxLength(4)]],
      targa: [this.data?.inventario.pneumatici.veicoloId, [Validators.required]],
      sede: [this.data?.inventario.deposito.sedeId, [Validators.required]],
      ubicazione: [this.data?.inventario.deposito.ubicazione, [Validators.required, Validators.maxLength(250)]],
      inizioDeposito: [this.data?.inventario.inizioDeposito, [Validators.required]],
      quantità: [this.data?.inventario.pneumatici.quantita, [Validators.required, Validators.min(1), Validators.max(8)]],
      statoGomme: [this.data?.inventario.statoGomme, [Validators.required]]
    });

    if (this.IsNewMode()) {
      this.pneumaticiForm.get('inizioDeposito')?.setValue(moment(new Date()));
    }
    else if (this.IsEditMode()) {
      this.pneumaticiForm.get('inizioDeposito')?.disable();
    }

    // load the initial list
    this.filtered.next(this.veicoli.slice());
    // listen for search field value changes
    this.filter.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => { this.filtering(); })

    this.currentUserid = this.userService.getUserID();
  }

  protected filtering() {
    if (this.veicoli.length == 0) { return; }
    // get the search keyword
    let search = this.filter.value;
    if (!search) {
      this.filtered.next(this.veicoli.slice());
      return;
    }
    else { search = search.toLowerCase(); }
    // filtering
    this.filtered.next(
      this.veicoli.filter(type => type.targa && type.targa.toLowerCase().indexOf(search) > -1)
    );
  }

  getStagioni(): void {
    this.subscribers.push(this.stagioniService.getAllStagioni().subscribe(
      (res: Response) => {
        this.stagioni = res.content;
      },
      err => {
        this.stagioni.length = 0;
        this.stagioni.splice(0);
      }
    ));
  }

  getSedi(): void {
    this.subscribers.push(this.sediService.getAllSedi().subscribe(
      (res: Response) => {
        this.sedi = res.content;
      },
      err => {
        this.sedi.length = 0;
        this.sedi.splice(0);
      }
    ));
  }

  getVeicoli(): void {
    this.subscribers.push(this.veicoliService.getAllVeicoliForSelectList().subscribe(
      (res: Response) => {
        this.veicoli = res.content;
        this.filtered.next(this.veicoli);
      },
      err => {
        this.veicoli.length = 0;
        this.veicoli.splice(0);
      }
    ));
  }

  addVeicolo() {
    let width;
    if (this.getWidth() < 400) { width = '90%'; }
    else { width = '60%'; }

    const dialogRef = this.dialog.open(ModalVeicoliComponent, {
      width: width,
      maxWidth: '90%',
      data: { mode: Mode.New, veicolo: new Veicolo(0, null, null, null, null, false, null) }
    });

    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); this.data.inventario.pneumatici.veicoloId = result.veicoloId; });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); this.data.inventario.pneumatici.veicoloId = result.veicoloId; });
  }

  removeVeicolo() {
    this.pneumaticiForm.get('targa')?.setValue(moment(0));
  }

  submitForm() {
    if (this.pneumaticiForm.invalid) { return; }

    this.deposito = {
      depositoId: 0,
      sedeId: this.pneumaticiForm.value.sede,
      ubicazione: this.pneumaticiForm.value.ubicazione,
      isDeleted: false,
      sede: null
    }

    this.pneumatici = {
      pneumaticiId: 0,
      marca: this.replaceSpacesInOne(this.pneumaticiForm.value.marca),
      modello: this.replaceSpacesInOne(this.pneumaticiForm.value.modello),
      misura: this.upperCase(this.pneumaticiForm.value.misura),    
      dot: this.pneumaticiForm.value.dot,
      stagioneId: this.pneumaticiForm.value.stagioneId,
      veicoloId: this.pneumaticiForm.value.targa,
      dataUbicazione: this.pneumaticiForm.value.inizioDeposito,
      quantita: this.pneumaticiForm.value.quantità,
      isDeleted: false,
      stagione: null,
      veicolo: null
    };

    this.inventario = {
      isDeleted: false,
      depositoId: 0,
      user: null,
      pneumaticiId: 0,
      inizioDeposito: this.pneumaticiForm.value.inizioDeposito,
      fineDeposito: null,
      battistrada: this.pneumaticiForm.value.battistrada,
      statoGomme: this.pneumaticiForm.value.statoGomme,
      deposito: this.deposito,
      pneumatici: this.pneumatici,
      userId: this.currentUserid
    }

    if (this.IsNewMode()) {
      this.subscribers.push(this.pneumaticiService.addPneumatici(this.inventario).subscribe(
        (res: Response) => {
          this.inventario = res.content;
          this.dialogRef.close(this.inventario);
          this.toastr.success('Pneumatici aggiunti con successo!');
          this.modalPDF();
        }
      ));
    }
    else if (this.IsEditMode()) {
      this.inventario.pneumaticiId = this.data?.inventario.pneumaticiId;
      console.log(this.inventario);
      this.subscribers.push(this.pneumaticiService.editPneumatici(this.inventario).subscribe(
        (res: Response) => {
          this.inventario = res.content;
          this.dialogRef.close(this.inventario);
          this.toastr.success('Pneumatici aggiornati con successo!');
          this.modalPDF();
        }
      ));
    }
  }

  modalPDF() {
    if (this.hiddenPopUpPdf) { return; }
    Swal.fire({
      title: 'Salvare PDF?',
      icon: 'warning',
      input: 'checkbox',
      inputValue: 0,
      inputPlaceholder: 'Non mostrare più',
      confirmButtonText: 'Sì',
      showDenyButton: true,
      denyButtonText: 'No',
    }).then((result) => {
      if (result.value) {
        this.userService.hidePopUpPdf();
      };
      if (result.isConfirmed && this.inventario) {
        this.pneumaticiService.generatePdf(this.inventario);
      };
    });
  }

  closeDialog() {
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

  deactivatePneumatici(id: number) {
    Swal.fire({
      title: 'Confermare la cancellazione?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    })
      .then((result) => {
        if (result.value) {
          this.subscribers.push(this.pneumaticiService.deactivatePneumatici(id).subscribe(
            () => {
              this.ngOnInit();
              this.toastr.success('Pneumatici eliminati con successo!');
            }
          ));
        }
      });
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    this.getWidth();
  }
}
