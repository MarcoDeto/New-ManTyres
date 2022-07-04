import { Component, OnInit, OnDestroy, HostListener, DoCheck } from '@angular/core';
import { Subscription } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Mode } from '../../../../Shared/Models/mode.model';
import Swal from 'sweetalert2';
import { Role } from '../../../../Shared/Models/role.model';
import { Cliente } from '../../../../Shared/Models/clienti.model';
import { ModalClientiComponent } from '../modal-clienti/modal-clienti.component';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent } from '@angular/material/paginator';
import { ExcelService } from '../../../Services/excel.service';
import { saveAs } from 'file-saver';
import { AdminUserService } from 'src/app/Admin/Services/admin-user.service';

@Component({
  selector: 'app-clienti',
  templateUrl: './clienti.component.html',
  styleUrls: ['./clienti.component.scss']
})

export class ClientiComponent implements OnInit, DoCheck, OnDestroy {
  @HostListener('window:resize', ['$event'])
  onResize(event: Event) { this.ngOnInit(); }

  title = 'Clienti';
  subscribers: Subscription[];
  hoverAdd = false;
  caricamento = false;
  notFound = false;
  dbEmpty = false;
  displayedColumns: string[] = ['nome', 'actions'];
  data = new MatTableDataSource<Cliente>();

  roles: Role[] = [];
  clienti: Cliente[] = [];
  orderByNome = false;
  filter = '';

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
    private excelService: ExcelService,
    private toastr: ToastrService,
    private userService: AdminUserService,
  ) {
    this.subscribers = [];
  }

  ngOnInit() {

    this.getAll();;

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
      this.getAll();
    }
  }

  applyFilter(event: Event) {
    this.pageEvent.pageIndex = 0;
    this.skip = this.pageEvent.pageSize * this.pageEvent.pageIndex;
    this.take = this.pageEvent.pageSize;
    this.getAll();
  }

  getAll() {
    this.skip = this.pageEvent.pageSize * this.pageEvent.pageIndex;
    this.take = this.pageEvent.pageSize;
    this.notFound = false;
    this.dbEmpty = false;
    this.subscribers.push(
      this.userService.getAll(this.skip, this.take).subscribe({
        next: res => {
          this.clienti = res.content;
          this.checkList();
        },
        error: err => this.checkList()
      })
    );
  }

  infoCliente(data: Cliente) {
    const dialogRef = this.dialog.open(ModalClientiComponent, {
      width: '80%',
      data: { mode: Mode.View, cliente: data }
    });

    dialogRef.beforeClosed().subscribe(result => { this.getAll(); });
    dialogRef.afterClosed().subscribe(result => { this.getAll(); });
  }

  addCliente() {
    const dialogRef = this.dialog.open(ModalClientiComponent, {
      maxWidth: '90%', width: '90%',
      data: { mode: Mode.New, cliente: new Cliente(0, null, null, null, null, null, null, null, null, null, null, null, null, false, false) }
    });

    dialogRef.beforeClosed().subscribe(result => { this.getAll(); });
    dialogRef.afterClosed().subscribe(result => { this.getAll(); });
  }

  editCliente(data: Cliente, event: Event)
  {
    event.stopPropagation();
    const dialogRef = this.dialog.open(ModalClientiComponent, {
      maxWidth: '90%', width: '90%',
      data: { mode: Mode.Edit, cliente: data }
    });

    dialogRef.beforeClosed().subscribe(result => { this.getAll(); });
    dialogRef.afterClosed().subscribe(result => { this.getAll(); });
  }

  deactivate(customerId: string, event: Event) {
    event.stopPropagation();
    Swal.fire({
      title: 'Confermare la cancellazione?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.value) {
        this.subscribers.push(
          this.userService.deactivateUser(customerId).subscribe(
            res => {
              this.getAll();
              this.toastr.success('Cliente eliminato con successo!');
            }
          )
        );
      }
    });
  }

  reactivate(data: any) {
    Swal.fire({
      title: 'Ripristinare cliente?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    })
      .then((result) => {
        if (result.value) {
          this.subscribers.push(
            this.userService.reactivateUser(data.id).subscribe(
              res => {
                this.getAll();
                this.toastr.success('Cliente eliminato con successo!');
              }
            )
          );
        }
      });
  }

  export() {
    this.subscribers.push(this.excelService.exportClienti().subscribe(
      res => {
        const blob = new Blob([res]);
        saveAs(blob, 'Clienti.xlsx');
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

  checkList() {
    if (!this.clienti || this.clienti.length == 0) { 
      this.dbEmpty = true; 
    }
    else { this.dbEmpty = false; }
  }
}
