import { Component, OnInit, OnDestroy, Inject, HostListener } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserPassword, Utenza } from 'src/app/Shared/Models/user.model';
import { Mode } from 'src/app/Shared/Models/mode.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AdminUserService } from '../../../Services/admin-user.service';
import { UtenzeComponent } from '../Utenze/utenze.component';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-utenze-modal',
  templateUrl: './modal-utenze.component.html',
  styleUrls: ['./modal-utenze.component.scss']
})

export class ModalUtenzeComponent implements OnInit, OnDestroy {
  subscribers: Subscription[] = [];
  user: Utenza | undefined;
  userPassword: UserPassword | undefined;
  role: string = "user";
  mode: Mode = Mode.New;
  hide: boolean = false; editPassword: boolean = false;
  ImgProfile: any;

  constructor(
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    public dialogRef: MatDialogRef<UtenzeComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public adminUserService: AdminUserService,
    private toastr: ToastrService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.ViewMode();
    this.dialogRef.disableClose = true;
    this.imagePath = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + this.data.user.imgProfile);
  }

  EditMode() { this.mode = Mode.Edit; }
  ViewMode() { this.mode = Mode.View; }
  IsEditMode() { return this.mode === Mode.Edit; }
  IsEditOrNewMode() { return this.mode === Mode.Edit || this.mode === Mode.New; }
  IsNewMode() { return this.mode === Mode.New; }
  IsViewMode() { return this.mode === Mode.View; }

  closeDialog() {
     this.dialogRef.close(null);
  }

  editUser(id: string) {
    this.router.navigate(['/admin/user', id]);
    this.closeDialog();
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
          this.toastr.info('operazione in corso');
          this.subscribers.push(this.adminUserService.deleteUser(id).subscribe(
            () => {
              this.closeDialog();
              this.toastr.clear();
              this.toastr.success('Utente eliminato con successo!');
            }
          ));
        }
      });
  }

  /*reactivateUser(data: Utenza) {
    Swal.fire({
      title: 'Ripristinare utente?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    })
      .then((result) => {
        if (result.value) {
          data.isDeleted = false;
          data.role = this.role;
          this.toastr.info('operazione in corso');
          this.subscribers.push(this.adminUserService.putUser(data).subscribe(
            () => {
              this.closeDialog();
              this.toastr.clear();
              this.toastr.success('Utente riattivato con successo!');
            }
          ));
        }
      });
  }
  */

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.getWidth();
  }

  //#region metodi per image
  imageClicked: boolean = false;
  toImage = 'data:image/jpg;base64';
  imagePath: any;
  image_url = '';

  onSelectFile(event: any) {
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();

      reader.onload = (event: any) => {
        this.image_url = event.target.result;
        this.setImagePath(this.image_url);
      }

      reader.readAsDataURL(event.target.files[0]);
    }

  }

  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl(url);
  }

  removeImage() {
    this.image_url = '';
    this.data.user.imgProfile = null;
  }

  setImagePath(url: string) {
    let path = url.toString().split(',');
    this.imagePath = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + path[1]);
    this.data.user.imgProfile = path[1];
  }

  //#endregion

  getWidth(): number {
    return window.innerWidth;
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }
}
