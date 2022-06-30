import { Component, OnDestroy, OnInit } from '@angular/core';
import { Form, FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import { User, UserPassword, Utenza } from '../../../../Shared/Models/user.model';
import { ValidatorsService } from '../../../../Shared/Validators/validators.services';
import { Mode } from '../../../../Shared/Models/mode.model';
import { AdminUserService } from '../../../Services/admin-user.service';
import { UserRole } from 'src/app/Shared/Models/enums';
import { PopUpNotificationService } from 'src/app/Shared/Services/popUpNotification.service';

export class ErrorPasswordStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const invalidParent = !!(
      control
      && control.parent
      && control.parent.invalid
      && control.parent.dirty
      && control.parent.hasError('notSame'));

    return invalidParent;
  }
}

@Component({
  selector: 'app-utenze-detail',
  templateUrl: './utenze-detail.component.html',
  styleUrls: ['./utenze-detail.component.scss']
})
export class UtenzeDetailComponent implements OnInit, OnDestroy {

  title = 'Dettagli Utente';
  userId = this.route.snapshot.paramMap.get('userId');
  subscribers: Subscription[] = [];
  user: User | null = null;
  userPassword: UserPassword | null = null;
  mode: Mode = Mode.New;
  hide = true;
  resetPassword = false;
  matcher = new ErrorPasswordStateMatcher();

  userForm = this.formBuilder.group({
    imgProfile: "",
    firstname: ["", [Validators.required, Validators.maxLength(50), this.validators.fullName()]],
    lastname: ["", [Validators.required, Validators.maxLength(50), this.validators.fullName()]],
    username: ["", [Validators.required, this.validators.userName()]],
    email: ["", [Validators.required, Validators.email]],
    phoneNumber: ["+39 ", [Validators.required, Validators.maxLength(20)]],
    password: ["", [Validators.required, Validators.minLength(8), this.validators.password()]],
    confermaPassword: ["", [Validators.required]],
    role: [UserRole.Worker, Validators.required]
  }, { validators: this.checkPasswords.bind(this) });
  get getUserControl() { return this.userForm.controls; }

  get Role() { return UserRole; }

  IsEditMode() { return this.mode === Mode.Edit; }
  IsNewMode() { return this.mode === Mode.New; }

  constructor(
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private validators: ValidatorsService,
    private toastr: ToastrService,
    public service: AdminUserService,
    private popup: PopUpNotificationService
  ) { }

  ngOnInit(): void {
    if (this.userId != null && this.userId != 'Add') {
      this.mode = Mode.Edit;
      this.subscribers.push(this.service.getUser(this.userId).subscribe(
        res => {
          this.user = res.content;
          this.userForm = this.formBuilder.group({
            id: [this.user?.id],
            imgProfile: [this.user?.imgProfile],
            username: [this.user?.userName, [Validators.required, this.validators.userName()]],
            firstname: [this.user?.firstName, [Validators.required, Validators.maxLength(50), this.validators.fullName()]],
            lastname: [this.user?.lastName, [Validators.required, Validators.maxLength(50), this.validators.fullName()]],
            email: [this.user?.email, [Validators.required, Validators.email]],
            phoneNumber: [this.user?.phoneNumber ? this.user?.phoneNumber : '+39', [Validators.required, Validators.maxLength(20)]],
            // password: ["", [Validators.minLength(8), this.validators.password()]],
            // confermaPassword: [""],
            isDeleted: false,
            role: this.user?.role
          }, { validators: this.checkPasswords.bind(this) });

          if (this.user?.photoUrl) {
            this.imagePath = this.user?.photoUrl;
          }
          else if (this.user?.imgProfile) {
            // IMAGE SANITAZER
            this.imagePath = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + this.userForm.value.imgProfile);
          }
        }
      ));
    }
  }

  DisableSubmit() {
    if (this.IsNewMode() && this.userForm.invalid)
      return true;
    else
      return false;
  }

  toPascalCase(text: string): string {
    if (!text) return text;
    text = this.replaceSpacesInOne(text);
    const splitted = text.split(' ');
    text = '';
    let i = 0;
    splitted.forEach(x => {
      i++;
      x = this.toTitleCase(x);
      if (i != splitted.length) {
        text += x + ' ';
      } else {
        text += x;
      }
    });
    return text;
  }

  replaceSpacesInOne(text: string): string {
    if (!text) return text;
    return text.trim().replace(/(?:\s+)\s/g, " ");
  }

  toTitleCase(text: string): string {
    if (!text) return text;
    return text.charAt(0).toUpperCase() + text.substr(1).toLowerCase();
  }

  OnSubmit() {

    if (this.userForm.invalid) { return; }

    if (this.IsNewMode()) {
      this.subscribers.push(this.service.postUser(this.userForm.value).subscribe(
        res => {
          this.toastr.success('Utente creato con successo!');
          this.router.navigate(['admin/users']);
        }
      ));
    }
    else {
      if (this.resetPassword == true) {
        this.userPassword = {
          id: this.user!.id,
          userName: this.user!.userName,
          password: "",
          newPassword: this.userForm.value.confermaPassword
        };
        this.subscribers.push(this.service.editPassword(this.userPassword!).subscribe(
          res => {
            console.log(res);
            this.toastr.success("Reset password avvenuto con successo");
            this.subscribers.push(this.service.putUser(this.userForm.value).subscribe(
              res => {
                this.toastr.success('Utente modificato con successo!');
                this.router.navigate(['admin/users']);
              }
            ));
          }
        ));
      }
      else {
        this.userForm.get('firstname')!.setValue(this.toPascalCase(this.userForm.value.firstname));
        this.userForm.get('lastname')!.setValue(this.toPascalCase(this.userForm.value.lastname));
        this.subscribers.push(this.service.putUser(this.userForm.value).subscribe(
          res => {
            this.popup.show(res.message);
            this.router.navigate(['admin/users']);
          }
        ));
      }
    }
  }

  changeValue(value: any) {
    this.hide = !value;
  }

  checkPasswords(group: FormGroup) {
    const password = group.value.password;
    const confirmPassword = group.value.confermaPassword;

    if (confirmPassword != "")
      return password === confirmPassword ? null : { notSame: true }

    return { notSame: true }
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
    Swal.fire({
      title: 'Rimuovere immagine?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.value) {
        this.image_url = '';
        this.userForm.get('imgProfile')!.setValue(null);
        this.imagePath = null;
      }
    });
  }

  setImagePath(url: string) {
    let path = url.toString().split(',');
    this.imagePath = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + path[1]);
    this.userForm.get('imgProfile')!.setValue(path[1]);
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

  goToLink(link: string | null) {
    this.router.navigate([link]);
  }
}
