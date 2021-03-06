import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, DoCheck, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Inventario } from '../../../Shared/Models/inventario.model';
import { InventarioService } from '../../Services/inventario.service';
import { Response } from 'src/app/Shared/Models/response.model';
import { PageEvent } from '@angular/material/paginator';
import { Sedi } from '../../../Shared/Models/sedi.model';
import { Stagioni } from '../../../Shared/Models/stagioni.model';
import { SediService } from '../../../Admin/Services/sedi.service';
import { StagioniService } from '../../Services/stagioni.service';
import { FormBuilder, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { PneumaticiService } from '../../Services/pneumatici.service';
import { Veicolo } from '../../../Shared/Models/veicoli.mdel';
import { VeicoliService } from '../../Services/veicoli.service';
import { saveAs } from 'file-saver';
import { Cliente } from '../../../Shared/Models/clienti.model';
import { MatDialog } from '@angular/material/dialog';
import { ModalClientiComponent } from '../clienti/modal-clienti/modal-clienti.component';
import { Mode } from '../../../Shared/Models/mode.model';
import { ModalPneumaticiComponent } from '../pneumatici/modal-pneumatici/modal-pneumatici.component';
import { FineDepositoComponent } from '../pneumatici/fine-deposito/fine-deposito.component';
import { InizioDepositoComponent } from '../pneumatici/inizio-deposito/inizio-deposito.component';
import { ExcelService } from '../../Services/excel.service';

@Component({
  selector: 'app-inventario',
  templateUrl: './inventario.component.html',
  styleUrls: ['./inventario.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0', display: 'none' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})

export class InventarioComponent implements OnInit, DoCheck, OnDestroy {
  title = 'Inventario';
  subscribers: Subscription[] = [];

  lista: Inventario[] = [];
  columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'inizioDeposito', 'userId', 'actions'];
  expandedElement: Inventario | undefined;
  veicolo: Veicolo | undefined;
  sedi: Sedi[] = [];
  stagioni: Stagioni[] = [];
  notFound = false;
  dbEmpty = false;
  pageEvent: PageEvent = {
    length: 0,
    pageIndex: 0,
    pageSize: 10,
    previousPageIndex: 0
  }
  length = 0;
  skip = 0;
  take = 10;
  sede = 0;
  stagione = 0;
  inizioOrderByDesc: boolean | null = true;
  fineOrderByDesc: boolean | null = null;

  inventario = false;
  archivio = false;
  cestino = false;
  filtri = false;

  inventarioForm = this.formBuilder.group({
    inizioOrderByDesc: this.inizioOrderByDesc,
    fineOrderByDesc: this.fineOrderByDesc,
    sede: this.sede,
    stagione: this.stagione
  });

  searchForm = this.formBuilder.group({
    targa: ['', [Validators.required]]
  });

  caricamento = false;

  constructor(
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private toastr: ToastrService,
    private sediService: SediService,
    private stagioniService: StagioniService,
    private inventarioService: InventarioService,
    private veicoliService: VeicoliService,
    private pneumaticiService: PneumaticiService,
    private excelService: ExcelService,
    private formBuilder: FormBuilder,
  ) {
    this.subscribers = [];
  }

  ngOnInit(): void {
    this.getStagioni();
    this.getSedi();
    this.skip = this.pageEvent.pageSize * this.pageEvent.pageIndex;
    this.take = this.pageEvent.pageSize;
    this.getInventario();
    if (this.getWidth() > 992)
      this.filtri = true;

  }

  ngDoCheck(): void {
    if (this.skip != this.pageEvent.pageSize * this.pageEvent.pageIndex || this.take != this.pageEvent.pageSize) {
      this.skip = this.pageEvent.pageSize * this.pageEvent.pageIndex;
      this.take = this.pageEvent.pageSize;
      if (this.searchForm.value.targa) {
        this.search()
      }
      else { this.submitForm(); }
    }
  }

  getAllDetail(veicoloId: number): void {
    console.log('Get All Detail');
    this.subscribers.push(this.veicoliService.getVeicolo(veicoloId).subscribe(
      res => this.veicolo = res.content
    ));
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

  search() {
    this.sede = this.inventarioForm.value.sede;
    this.stagione = this.inventarioForm.value.stagione;
    this.inizioOrderByDesc = this.inventarioForm.value.inizioOrderByDesc;
    this.fineOrderByDesc = this.inventarioForm.value.fineOrderByDesc;
    if (this.inventario)
      this.getInventarioByTarga(this.skip, this.take, this.sede, this.stagione, this.inizioOrderByDesc!, this.searchForm.value.targa);
    if (this.archivio)
      this.getArchivioByTarga(this.skip, this.take, this.sede, this.stagione, this.inizioOrderByDesc, this.fineOrderByDesc, this.searchForm.value.targa);
    if (this.cestino)
      this.getCestinoByTarga(this.skip, this.take, this.sede, this.stagione, this.inizioOrderByDesc, this.fineOrderByDesc, this.searchForm.value.targa);
  }

  getInventario() {
    this.notFound = false;
    this.dbEmpty = false;
    this.inventario = true;
    this.archivio = false;
    this.cestino = false;
    this.fineOrderByDesc = null;
    this.inventarioForm.get('fineOrderByDesc')?.setValue(null);
    this.inizioOrderByDesc = this.inventarioForm.value.inizioOrderByDesc;
    this.sede = this.inventarioForm.value.sede;
    this.stagione = this.inventarioForm.value.stagione;
    this.columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'inizioDeposito', 'userId', 'actions'];
    if (this.getWidth() < 415) { this.columnsToDisplay = ['N', 'pneumaticiId', 'actions']; }
    else if (this.getWidth() < 767) { this.columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'actions']; }
    this.subscribers.push(
      this.inventarioService.getInventario(this.skip, this.take, this.sede, this.stagione, this.inventarioForm.value.inizioOrderByDesc).subscribe({
        next: (res: Response) => {
          this.lista = res.content;
          this.length = res.count;
          if (res.count == 0) { this.dbEmpty = true; }
        },
        error: err => {
          this.notFound = true;
        }
      }));
  }

  getInventarioByTarga(skip: number, take: number, sede: number, stagione: number, orderByDesc: boolean, targa: string) {
    this.notFound = false;
    this.dbEmpty = false;
    this.inventario = true;
    this.archivio = false;
    this.cestino = false;
    this.columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'inizioDeposito', 'userId', 'actions'];
    if (this.getWidth() < 415) { this.columnsToDisplay = ['N', 'pneumaticiId', 'actions']; }
    else if (this.getWidth() < 767) { this.columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'actions']; }
    this.subscribers.push(
      this.inventarioService.getInventarioByTarga(skip, take, sede, stagione, orderByDesc, targa).subscribe({
        next: (res: Response) => {
          this.lista = res.content;
          this.length = res.count;
          if (res.count == 0) { this.dbEmpty = true; }
        },
        error: err => {
          this.notFound = true;
        }
      }));
  }

  getArchivio(skip: number, take: number, sede: number, stagione: number, inizioOrderByDesc: boolean | null, fineOrderByDesc: boolean | null) {
    this.notFound = false;
    this.dbEmpty = false;
    this.inventario = false;
    this.archivio = true;
    this.cestino = false;
    this.columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'inizioDeposito', 'fineDeposito', 'userId', 'actions'];
    if (this.getWidth() < 415) { this.columnsToDisplay = ['N', 'pneumaticiId', 'actions']; }
    else if (this.getWidth() < 767) { this.columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'actions']; }
    this.subscribers.push(
      this.inventarioService.getArchivio(skip, take, sede, stagione, inizioOrderByDesc, fineOrderByDesc).subscribe({
        next: (res: Response) => {
          this.lista = res.content;
          this.length = res.count;
          if (res.count == 0) { this.dbEmpty = true; }
        },
        error: err => {
          this.notFound = true;
        }
      }));
  }

  getArchivioByTarga(skip: number, take: number, sede: number, stagione: number, inizioOrderByDesc: boolean | null, fineOrderByDesc: boolean | null, targa: string) {
    this.notFound = false;
    this.dbEmpty = false;
    this.inventario = false;
    this.archivio = true;
    this.cestino = false;
    this.columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'inizioDeposito', 'fineDeposito', 'userId', 'actions'];
    if (this.getWidth() < 415) { this.columnsToDisplay = ['N', 'pneumaticiId', 'actions']; }
    else if (this.getWidth() < 767) { this.columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'actions']; }
    this.subscribers.push(
      this.inventarioService.GetArchivioByTarga(skip, take, sede, stagione, inizioOrderByDesc, fineOrderByDesc, targa).subscribe({
        next: (res: Response) => {
          this.lista = res.content;
          this.length = res.count;
          if (res.count == 0) { this.dbEmpty = true; }
        },
        error: err => {
          this.notFound = true;
        }
      }));
  }

  DelFromArchivio(entity: Inventario) {
    Swal.fire({
      title: 'Confermare la cancellazione?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'S??',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.value) {
        this.subscribers.push(this.inventarioService.DelFromArchivio(entity).subscribe(
          res => {
            this.toastr.success('Spostato nel cestino!');
            this.submitForm();
          }
        ));
      }
    });
  }

  getCestino(skip: number, take: number, sede: number, stagione: number, inizioOrderByDesc: boolean | null, fineOrderByDesc: boolean | null ) {
    this.notFound = false;
    this.dbEmpty = false;
    this.inventario = false;
    this.archivio = false;
    this.cestino = true;
    this.columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'inizioDeposito', 'fineDeposito', 'userId', 'actions'];
    if (this.getWidth() < 415) { this.columnsToDisplay = ['N', 'pneumaticiId', 'actions']; }
    else if (this.getWidth() < 767) { this.columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'actions']; }
    this.subscribers.push(
      this.inventarioService.getCestino(skip, take, sede, stagione, inizioOrderByDesc, fineOrderByDesc).subscribe({
        next: (res: Response) => {
          this.lista = res.content;
          this.length = res.count;
          if (res.count == 0) { this.dbEmpty = true; }
        },
        error: err => {
          this.notFound = true;
        }
      })
    );
  }

  getCestinoByTarga(skip: number, take: number, sede: number, stagione: number, inizioOrderByDesc: boolean | null, fineOrderByDesc: boolean | null, targa: string) {
    this.notFound = false;
    this.dbEmpty = false;
    this.inventario = false;
    this.archivio = false;
    this.cestino = true;
    this.columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'inizioDeposito', 'fineDeposito', 'userId', 'actions'];
    if (this.getWidth() < 415) { this.columnsToDisplay = ['N', 'pneumaticiId', 'actions']; }
    else if (this.getWidth() < 767) { this.columnsToDisplay = ['N', 'pneumaticiId', 'depositoId', 'actions']; }
    this.subscribers.push(
      this.inventarioService.getCestinoByTarga(skip, take, sede, stagione, inizioOrderByDesc, fineOrderByDesc, targa).subscribe({
        next: (res: Response) => {
          this.lista = res.content;
          this.length = res.count;
          if (res.count == 0) { this.dbEmpty = true; }
        },
        error: err => {
          this.notFound = true;
        }
      })
    );
  }

  DelFromCestino(entity: Inventario) {
    Swal.fire({
      title: 'Confermare la cancellazione?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'S??',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.value) {
        this.subscribers.push(this.inventarioService.DelFromCestino(entity).subscribe(
          res => {
            this.toastr.success('Spostato nel cestino! ');
            this.submitForm();
          }
        ));
      }
    });
  }

  RipristinaFromCestino(entity: Inventario) {
    Swal.fire({
      title: 'Confermare il ripristino?',
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'S??',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.value) {
        this.subscribers.push(this.inventarioService.RipristinaFromCestino(entity).subscribe(
          res => {
            this.toastr.success('Spostato in archivio!');
            this.submitForm();
          }
        ));
      }
    });
  }

  RipristinaCestino() {
    Swal.fire({
      title: 'Sicuro di voler ripristinare tutto il cestino?',
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'S??',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.isConfirmed) {
        this.subscribers.push(this.inventarioService.RipristinaCestino().subscribe(
          res => {
            this.toastr.success('Cestino ripristinato!');
            this.submitForm();
          }
        ));
      }
    });
  }

  SvuotaCestino() {
    Swal.fire({
      title: 'I dati saranno eliminati definitivamente!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Continua',
      cancelButtonText: 'Annulla'
    }).then((result) => {
      if (result.isConfirmed) {
        this.subscribers.push(this.inventarioService.SvuotaCestino().subscribe(
          res => {
            this.toastr.success('Cestino vuoto!');
            this.submitForm();
          }
        ));
      }
    });
  }

  clickOnInizioOrder() {
    this.fineOrderByDesc = null;
    this.inventarioForm.get('fineOrderByDesc')?.setValue(null);
    this.submitForm();
  }

  clickOnFineOrder() {
    this.inizioOrderByDesc = null;
    this.inventarioForm.get('inizioOrderByDesc')?.setValue(null);
    this.submitForm();
  }

  submitForm() {
    this.sede = this.inventarioForm.value.sede;
    this.stagione = this.inventarioForm.value.stagione;
    this.inizioOrderByDesc = this.inventarioForm.value.inizioOrderByDesc;
    this.fineOrderByDesc = this.inventarioForm.value.fineOrderByDesc;

    if (this.inventario)
      this.getInventario();
    if (this.archivio)
      this.getArchivio(this.skip, this.take, this.inventarioForm.value.sede, this.inventarioForm.value.stagione, this.inventarioForm.value.inizioOrderByDesc, this.inventarioForm.value.fineOrderByDesc);
    else if (this.cestino)
      this.getCestino(this.skip, this.take, this.inventarioForm.value.sede, this.inventarioForm.value.stagione, this.inventarioForm.value.inizioOrderByDesc, this.inventarioForm.value.fineOrderByDesc);
  }

  Pdf(data: Inventario) {
    this.pneumaticiService.generatePdf(data);
  }

  infoCliente(data: Cliente) {
    const dialogRef = this.dialog.open(ModalClientiComponent, {
      width: '80%',
      data: { mode: Mode.View, cliente: data }
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
        dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
        dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
      }
      if (result.isDenied) {
        if (data.fineDeposito == null) {
          const dialogRef = this.dialog.open(FineDepositoComponent, {
            maxWidth: '90%',
            data: { mode: Mode.Edit, inventario: data }
          });
          dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
          dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
        }
        else {
          const dialogRef = this.dialog.open(InizioDepositoComponent, {
            maxWidth: '90%',
            data: { mode: Mode.Edit, inventario: data }
          });
          dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
          dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
        }
      }
    });
  }

  export(): void {
    this.sede = this.inventarioForm.value.sede;
    this.stagione = this.inventarioForm.value.stagione;
    this.inizioOrderByDesc = this.inventarioForm.value.inizioOrderByDesc;
    this.fineOrderByDesc = this.inventarioForm.value.fineOrderByDesc;

    let sedeMess: string | null = ' sede = '
    if (this.sede != 0) {
      this.sedi.forEach(x => {
        if (x.sedeId == this.sede && sedeMess) {
          sedeMess += x.comune;
        }
      });
    }
    else { sedeMess = null; }
    let stagioneMess: string | null = ' stagione = '
    if (this.stagione != 0) {
      this.stagioni.forEach(x => {
        if (x.stagioneId == this.stagione) {
          stagioneMess += x.nome;
        }
      });
    }
    else { stagioneMess = null; }

    let message = 'Esportare i dati '
    let filtriMess = '';
    if (stagioneMess != null) { filtriMess += 'filtrati per' + stagioneMess; }
    if (sedeMess != null && stagioneMess == null) { filtriMess += 'filtrati per' + sedeMess; }
    if (sedeMess != null && stagioneMess != null) { filtriMess += ',' + sedeMess; }

    if (filtriMess != '') { message += filtriMess + ' e '; }
    let orderMess = this.inizioOrderByDesc == true ? 'ordinati per inizio deposito pi?? recente' : 'ordinati per inizio deposito pi?? vecchio';
    if (this.fineOrderByDesc == true) { orderMess = 'ordinati per fine deposito pi?? recente'; }
    else if (this.fineOrderByDesc == false) { orderMess = 'ordinati per fine deposito pi?? vecchio'; }
    message += orderMess;
    message += '?';
    Swal.fire({
      title: message,
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Scarica',
      cancelButtonText: 'Annulla'
    }).then((result) => {
      if (result.isConfirmed) {
        if (this.inventario) { this.exportInventario(this.sede, this.stagione, this.inizioOrderByDesc!) }
        else if (this.archivio) { this.exportArchivio(this.sede, this.stagione, this.inizioOrderByDesc, this.fineOrderByDesc) }
        else if (this.cestino) { this.exportPneumatici(this.sede, this.stagione, this.inizioOrderByDesc!) }
      }
    });
  }

  exportPneumatici(sede: number, stagione: number, orderByDesc: boolean): void {
    this.caricamento = true;
    this.subscribers.push(
      this.excelService.exportPneumatici(sede, stagione, orderByDesc).subscribe({
        next: (res: any) => {
          const blob = new Blob([res]);
          saveAs(blob, 'Tyres.xlsx');
          this.caricamento = false;
        },
        error: err => {
          this.caricamento = false;
        }
      })
    );
  }

  exportInventario(sede: number, stagione: number, orderByDesc: boolean): void {
    this.caricamento = true;
    this.subscribers.push(
      this.excelService.exportInventario(sede, stagione, orderByDesc).subscribe({
        next: (res: any) => {
          const blob = new Blob([res]);
          saveAs(blob, 'Tyres.xlsx');
          this.caricamento = false;
        },
        error: err => {
          this.caricamento = false;
        }
      })
    );
  }

  exportArchivio(sede: number, stagione: number, inizioOrderByDesc: boolean | null, fineOrderByDesc: boolean | null): void {
    this.caricamento = true;
    this.subscribers.push(
      this.excelService.exportArchivio(sede, stagione, inizioOrderByDesc, fineOrderByDesc).subscribe({
        next: (res: any) => {
          const blob = new Blob([res]);
          saveAs(blob, 'Tyres.xlsx');
          this.caricamento = false;
        },
        error: err => {
          this.caricamento = false;
        }
      })
    );
  }

  getWidth(): number {
    return window.innerWidth;
  }

  showfiltri() {
    this.filtri = !this.filtri;
    console.log(this.filtri);
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }
}
