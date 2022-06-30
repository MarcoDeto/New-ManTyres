import { Component, OnInit, Output, ViewChild, AfterViewInit, OnDestroy, HostListener, DoCheck } from '@angular/core';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Response } from 'src/app/Shared/Models/response.model';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Mode } from '../../../../Shared/Models/mode.model';
import Swal from 'sweetalert2';
import { Role } from '../../../../Shared/Models/role.model';
import { DomSanitizer } from '@angular/platform-browser';
import { ModalVeicoliComponent } from '../modal-veicoli/modal-veicoli.component';
import { Veicolo } from '../../../../Shared/Models/veicoli.mdel';
import { VeicoliService } from '../../../Services/VeicoliService';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { ExcelService } from '../../../Services/Excel.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-veicoli',
  templateUrl: './veicoli.component.html',
  styleUrls: ['./veicoli.component.scss']
})

export class VeicoliComponent implements OnInit, DoCheck, OnDestroy {
  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.ngOnInit();
  }

  title = 'Veicoli';
  subscribers: Subscription[];
  caricamento = false; notFound = false; dbEmpty = false;
  displayedColumns: string[];
  data = new MatTableDataSource<Veicolo>();
  editing = false; deleting = false;

  roles: Role[];
  veicoli: Veicolo[];
  orderByTarga = false;
  targa = "";

  pageEvent: PageEvent = {
    length: 0,
    pageIndex: 0,
    pageSize: 10,
    previousPageIndex: 0
  }
  length = 0;
  skip = 0;
  take = 10;

  constructor(
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private http: HttpClient,
    public dialog: MatDialog,
    private veicoliService: VeicoliService,
    private excelService: ExcelService,
    private toastr: ToastrService,
  ) {
    this.subscribers = [];
  }

  ngOnInit() {
    this.skip = this.pageEvent.pageSize * this.pageEvent.pageIndex;
    this.take = this.pageEvent.pageSize;
    this.getAll(this.skip, this.take, this.orderByTarga, this.targa);

    if (this.getWidth() > 800) {
      this.displayedColumns = ['targa', 'marca', 'modello', 'cliente', 'actions'];
    }
    else if (this.getWidth() > 415) {
      this.displayedColumns = ['targa', 'marca', 'modello', 'actions'];
    }
    else {
      this.displayedColumns = ['targa'];
    }
  }

  ngDoCheck(): void {
    if (this.skip != this.pageEvent.pageSize * this.pageEvent.pageIndex || this.take != this.pageEvent.pageSize) {
      this.skip = this.pageEvent.pageSize * this.pageEvent.pageIndex;
      this.take = this.pageEvent.pageSize;
      this.getAll(this.skip, this.take, this.orderByTarga, this.targa);
    }
  }

  orderBy() {
    this.orderByTarga = !this.orderByTarga;
    console.log(this.orderByTarga);
    this.getAll(this.skip, this.take, this.orderByTarga, this.targa)
  }

  applyFilter(event: Event) {
    this.pageEvent.pageIndex = 0;
    this.skip = this.pageEvent.pageSize * this.pageEvent.pageIndex;
    this.take = this.pageEvent.pageSize;
    this.getAll(this.skip, this.take, this.orderByTarga, this.targa)
  }


  /********* CRUD METHODS *********/
  getAll(skip: number, take: number, orderByTarga: boolean, targa: string) {
    this.notFound = false;
    this.dbEmpty = false;

    this.subscribers.push(this.veicoliService.getAllSkipTake(skip, take, orderByTarga, targa).subscribe(
      (res: Response) => {
        this.veicoli = res.content;
        this.length = res.count;
        if (res.count == 0) { this.dbEmpty = true; }
      },
      err => {
        this.notFound = true;
      }
    ));
  }

  infoVeicolo(data: Veicolo) {
    console.log(data);
    const dialogRef = this.dialog.open(ModalVeicoliComponent, {
      width: '80%',
      data: { mode: Mode.View, veicolo: data }
    });
    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
  }

  addVeicolo() {
    const dialogRef = this.dialog.open(ModalVeicoliComponent, {
      maxWidth: '90%',
      data: { mode: Mode.New, veicolo: new Veicolo(0, null, null, null, null, false, null, null) }
    });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
  }

  editVeicolo(data: Veicolo, event: Event) {
    if (event != null) { event.stopPropagation(); }
    const dialogRef = this.dialog.open(ModalVeicoliComponent, {
      data: { mode: Mode.Edit, veicolo: data }
    });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
  }

  deactivateVeicolo(id: number, event: Event) {
    if (event != null) { event.stopPropagation(); }
    Swal.fire({
      title: 'Confermare la cancellazione?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    })
      .then((result) => {
        if (result.isConfirmed) {
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
        if (result.isConfirmed) {
          this.subscribers.push(this.veicoliService.reactivateVeicolo(id).subscribe(
            () => {
              this.ngOnInit();
              this.toastr.success('Veicolo riattivato con successo!');
            }
          ));
        }
      });
  }
  /******* END CRUD METHODS *******/

  export() {
    this.subscribers.push(this.excelService.exportVeicoli().subscribe(
      res => {
        const blob = new Blob([res]);
        saveAs(blob, "Veicoli.xlsx");
      }
    ));
  }

  getWidth(): number {
    return window.innerWidth;
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = null;
  }
}
