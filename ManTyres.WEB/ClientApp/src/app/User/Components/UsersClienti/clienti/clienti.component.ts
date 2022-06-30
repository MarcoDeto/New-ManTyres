import { Component, OnInit, OnDestroy, HostListener, DoCheck } from '@angular/core';
import { Subscription } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Mode } from '../../../../Shared/Models/mode.model';
import Swal from 'sweetalert2';
import { Role } from '../../../../Shared/Models/role.model';
import { DomSanitizer } from '@angular/platform-browser';
import { Cliente } from '../../../../Shared/Models/clienti.model';
import { ClientiService } from '../../../Services/ClientiService';
import { ModalClientiComponent } from '../modal-clienti/modal-clienti.component';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent } from '@angular/material/paginator';
import { ExcelService } from '../../../Services/Excel.service';
import { saveAs } from 'file-saver';
import { FirstConnectionService } from '../../../../Shared/Services/FirstConnection.service';

@Component({
  selector: 'app-clienti',
  templateUrl: './clienti.component.html',
  styleUrls: ['./clienti.component.scss']
})

export class ClientiComponent implements OnInit, DoCheck, OnDestroy {
  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.ngOnInit();
  }

  title = 'Clienti';
  subscribers: Subscription[];
  hoverAdd = false;
  caricamento = false;
  notFound = false;
  dbEmpty = false;
  displayedColumns: string[] = ['nome', 'actions'];
  data = new MatTableDataSource<Cliente>();

  roles: Role[];
  clienti: Cliente[];
  orderByNome = false;
  filter = "";

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
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private clientiService: ClientiService,
    private excelService: ExcelService,
    private toastr: ToastrService,
    private dataService: FirstConnectionService,
  ) {
    this.subscribers = [];
  }

  ngOnInit() {
    this.skip = this.pageEvent.pageSize * this.pageEvent.pageIndex;
    this.take = this.pageEvent.pageSize;
    this.getAll(this.skip, this.take, this.orderByNome, this.filter);

    if (this.getWidth() > 415) {
      this.displayedColumns = ['nome', 'actions'];
    }
    else {
      this.displayedColumns = ['nome'];
    }
  }

  ngDoCheck(): void {
    if (this.skip != this.pageEvent.pageSize * this.pageEvent.pageIndex || this.take != this.pageEvent.pageSize) {
      this.skip = this.pageEvent.pageSize * this.pageEvent.pageIndex;
      this.take = this.pageEvent.pageSize;
      this.getAll(this.skip, this.take, this.orderByNome, this.filter);
    }
  }

  applyFilter(event: Event) {
    this.pageEvent.pageIndex = 0;
    this.skip = this.pageEvent.pageSize * this.pageEvent.pageIndex;
    this.take = this.pageEvent.pageSize;
    this.getAll(this.skip, this.take, this.orderByNome, this.filter)
  }

  getAll(skip: number, take: number, orderByNome: boolean, filter: string) {
    this.notFound = false;
    this.dbEmpty = false;
    this.clienti = this.dataService.clienti.filter(x => x.nome.includes(filter))
      .sort((n1, n2) => {
        if (n1.clienteId > n2.clienteId) { return -1; }
        if (n1.clienteId < n2.clienteId) { return 1; }
        return 0;
      }).splice(skip, take);
    if (this.clienti.length == 0) { this.dbEmpty = true; }
    else { this.dbEmpty = false; }
  }

  infoCliente(data: Cliente) {
    const dialogRef = this.dialog.open(ModalClientiComponent, {
      width: '80%',
      data: { mode: Mode.View, cliente: data }
    });

    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
  }

  addCliente() {
    const dialogRef = this.dialog.open(ModalClientiComponent, {
      maxWidth: '90%', width: '90%',
      data: { mode: Mode.New, cliente: new Cliente(0, null, null, null, null, null, null, null, null, null, null, null, null, null, false, null) }
    });

    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
  }

  editCliente(data: Cliente, event: Event)
  {
    event.stopPropagation();
    const dialogRef = this.dialog.open(ModalClientiComponent, {
      maxWidth: '90%', width: '90%',
      data: { mode: Mode.Edit, cliente: data }
    });

    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
  }

  deleteCliente(id: number) {
    event.stopPropagation();
    Swal.fire({
      title: 'Confermare la cancellazione?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.value) {
        const index = this.dataService.clienti.findIndex(x => x.clienteId == id);
        this.dataService.clienti.splice(index, 1);
        this.toastr.success('Cliente eliminato con successo!');
      }
    });
  }

  reactivateCliente(data) {
    Swal.fire({
      title: 'Ripristinare cliente?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    })
      .then((result) => {
        if (result.value) {
          let cliente = this.clienti.find(u => u.clienteId == data.ClienteId);
          cliente.isDeleted = false;
          this.subscribers.push(this.clientiService.editCliente(cliente).subscribe(
            () => {
              this.ngOnInit();
              this.toastr.success('Cliente riattivato con successo!');
            }
          ));
        }
      });
  }

  export() {
    this.subscribers.push(this.excelService.exportClienti().subscribe(
      res => {
        const blob = new Blob([res]);
        saveAs(blob, "Clienti.xlsx");
      }
    ));
  }

  getWidth(): number {
    return window.innerWidth;
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }
}
