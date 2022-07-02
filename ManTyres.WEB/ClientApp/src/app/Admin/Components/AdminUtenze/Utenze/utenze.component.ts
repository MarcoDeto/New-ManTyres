import { Component, OnInit, Output, ViewChild, AfterViewInit, OnDestroy, HostListener } from '@angular/core';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Users, Utenza } from 'src/app/Shared/Models/user.model';
import { AdminUserService } from '../../../Services/admin-user.service';
import { Response } from 'src/app/Shared/Models/response.model';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ModalUtenzeComponent } from '../modal-utenze/modal-utenze.component';
import { Mode } from '../../../../Shared/Models/mode.model';
import Swal from 'sweetalert2';
import { Role } from '../../../../Shared/Models/role.model';
import { RoleService } from '../../../Services/role.service';
import { DomSanitizer } from '@angular/platform-browser';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-utenze',
  templateUrl: './utenze.component.html',
  styleUrls: ['./utenze.component.scss']
})
export class UtenzeComponent implements OnInit, AfterViewInit, OnDestroy {// AfterContentInit, AfterContentChecked,  AfterViewChecked, OnChanges{
  title = 'Utenze';
  subscribers: Subscription[] = [];
  caricamento = false;
  notFound = false;
  dbEmpty = false;
  displayedColumns: string[] = [];
  data = new MatTableDataSource<Users>();
  role: string = "user";
  hoverAdd = false;

  users: Utenza[] = [];

  @ViewChild(MatSort) sort: MatSort | undefined;
  @ViewChild(MatPaginator) paginator: MatPaginator | undefined;

  constructor(
    public dialog: MatDialog,
    private route: ActivatedRoute,
    private adminUserService: AdminUserService,
    private toastr: ToastrService,
    private router: Router
  ) {
    this.subscribers = [];
    this.data = new MatTableDataSource();
  }

  ngOnInit() {
    this.getUsers();
    if (this.getWidth() > 800) {
      this.displayedColumns = ['firstName', 'lastName', 'phoneNumber', 'email', 'userName', 'actions'];
    }
    else if (this.getWidth() > 415) {
      this.displayedColumns = ['firstName', 'lastName', 'userName', 'actions'];
    }
    else {
      this.displayedColumns = ['firstName'];
    }
  }

  ngAfterViewInit() {
    this.data.sort = this.sort!;
    this.data.paginator = this.paginator!;
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.data.filter = filterValue.trim().toLowerCase();
  }

  getWidth(): number {
    return window.innerWidth;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    this.ngOnInit();
  }

  /********* CRUD METHODS *********/

  getUsers() {
    this.data.data.length = 0;
    this.data.data.splice(0);
    this.notFound = false;
    this.dbEmpty = false;
    this.caricamento = true;
    this.subscribers.push(this.adminUserService.getAllUsers().subscribe(
      (res: Response) => {
        this.caricamento = false;
        if (res.content == null) {
          this.dbEmpty = true;
          return;
        }
        this.users = res.content;
        this.populateDataSource(this.users);
      },
      err => {
        this.data.data.length = 0;
        this.data.data.splice(0);
        this.caricamento = false;
        this.notFound = true;
      }
    ));
  }

  addUser() {
    this.router.navigate(['/admin/user', 'Add']);
  }

  editUser(id: string) {
    this.router.navigate(['/admin/user', id]);
  }

  private populateDataSource(array: Array<Users>) {
    this.data.data = array as Users[];
  }

  infoUser(data: Utenza) {
    if (this.getWidth() < 415) {
      const dialogRef = this.dialog.open(ModalUtenzeComponent, {
        width: '80%',
        data: { mode: Mode.View, user: data }
      });
      dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
      dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
    }
  }

  deleteUser(id: string) {
    Swal.fire({
      title: 'Confermare la cancellazione?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    })
      .then((result) => {
        if (result.value) {
          this.subscribers.push(this.adminUserService.deleteUser(id).subscribe(
            () => {
              this.ngOnInit();
              this.toastr.success('Utente eliminato con successo!');
            }
          ));
        }
      });
  }

  /*reactivateUser(data) {
    Swal.fire({
      title: 'Ripristinare utente?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    })
      .then((result) => {
        if (result.value) {
          let user = this.users.find(u => u.id == data.id);
          user.isDeleted = false;
          user.role = this.role;
          this.subscribers.push(this.adminUserService.putUser(user).subscribe(
            () => {
              this.ngOnInit();
              this.toastr.success('Utente riattivato con successo!');
            }
          ));
        }
      });
  }*/

}



