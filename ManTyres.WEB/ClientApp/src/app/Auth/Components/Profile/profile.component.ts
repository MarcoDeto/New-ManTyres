import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { User } from '../../../Shared/Models/user.model';
import Swal from 'sweetalert2';
import { DomSanitizer } from '@angular/platform-browser';
import { Validators, FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { ValidatorsService } from '../../../Shared/Validators/validators.services';
import { UserService } from '../../Services/user.service';
import { Response } from '../../../Shared/Models/response.model';
import { UserRole } from 'src/app/Shared/Models/enums';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],

})
export class ProfileComponent implements OnInit, OnDestroy {
  title = 'Profilo';

  subscribers: Subscription[] = [];

  userForm = this.formBuilder.group({});
  get getUserControl() { return this.userForm.controls; }

  user: User = {
    city: null,
    companyName: null,
    concurrencyStamp: null,
    country: null,
    cultureInfo: null,
    email: null,
    emailConfirmed: false,
    firstName: null,
    imgProfile: null,
    lastName: null,
    passwordHash: null,
    phoneNumber: null,
    phoneNumberConfirmed: false,
    photoUrl: null,
    role: UserRole.Customer,
    securityStamp: null,
    socialUserId: null,
    street: null,
    twoFactorEnabled: false,
    userName: null,
    website: null,
    zipcode: null,
    id: undefined,
    createdAt: new Date(),
    updatedAt: new Date(),
    isDeleted: false,
    googleId: null,
    facebookId: null,
    taxCode: null,
    isBusiness: false,
    businessId: null,
    provider: null
  }
  caricamento = true;
  hide = true;

  currentUserID: string | undefined;

  constructor(
    private sanitizer: DomSanitizer,
    private userService: UserService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private validators: ValidatorsService,
    private formBuilder: FormBuilder,
  ) { }

  ngOnInit(): void {

    this.currentUserID = this.userService.getUserID()!;
    if (!this.currentUserID || this.currentUserID == 'undefined') {
      this.router.navigate(['/account/register']);
    }

    this.subscribers.push(
      this.userService.getById(this.currentUserID).subscribe({
        next: (res: Response) => {
          this.user = res.content;
          this.caricamento = false;
          this.userForm = this.formBuilder.group({
            id: [this.user.id],
            imgProfile: [this.user.imgProfile ? this.user.imgProfile : this.user.photoUrl],
            firstname: [this.user.firstName, [Validators.required, Validators.maxLength(50), this.validators.fullName()]],
            lastname: [this.user.lastName, [Validators.required, Validators.maxLength(50), this.validators.fullName()]],
            username: [this.user.userName, [Validators.required, this.validators.userName()]],
            email: [this.user.email, [Validators.required, Validators.email]],
            phoneNumber: [this.user.phoneNumber, [Validators.required, Validators.maxLength(20)]],
          });
          if (this.user.photoUrl) {
            this.imagePath = this.user.photoUrl;
          }
          else if (this.user.imgProfile) {
            // IMAGE SANITAZER
            this.imagePath = this.sanitizer.bypassSecurityTrustResourceUrl(
              'data:image/jpg;base64,' + this.userForm.value.imgProfile);
          }
        },
        error: error => {
          this.caricamento = false;
          if (error.error.code == 404) {
            this.userService.logout();
            this.router.navigate(['account/login']);
          }
          this.router.navigate(['/error']);
        }
      })
    );
  }

  OnSubmit() {
    this.caricamento = true;

    this.user.photoUrl = this.userForm.value.imgProfile;
    this.user.firstName = this.userForm.value.firstname;
    this.user.lastName = this.userForm.value.lastname;
    this.user.userName = this.userForm.value.username;
    this.user.email = this.userForm.value.email;
    this.user.phoneNumber = this.userForm.value.phoneNumber;

    this.subscribers.push(
      this.userService.putUser(this.user).subscribe({
        next: (res: Response) => {
          this.caricamento = false;
          if (res.content) {
            this.toastr.success('Profilo modificato');
          }
          else {
            this.toastr.error(res.message);
          }
        },
        error: err => {
          this.caricamento = false;
          this.toastr.error('Errore!' + err.error);
          console.error(err.error.errorMessage);
        }
      }));

    if (this.currentUserID != this.userForm.value.username) {
      Swal.fire({
        title: "Hai cambiato username, rifare l'accesso!",
        icon: 'warning',
        confirmButtonText: 'Ok',
      }).then((result) => {
        this.userService.removeToken();
        this.router.navigate(['account/login']);
        this.router.navigate(['']);
      });
    }
  }

  confirmEmail() {

  }


  getWidth(): number {
    return window.innerWidth;
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

  replaceSpacesInOne(text: string) {
    if (!text) return text;
    return text.replace(/(?:\s+)\s/g, ' ');
  }

  //#region metodi per image
  imageClicked: boolean = false;
  toImage = 'data:image/jpg;base64';
  imagePath: any | null;
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
        this.userForm.value.imgProfile.setValue(null);
        //this.userForm.get('imgProfile')?.setValue(null);
      }
    });
  }

  setImagePath(url: string) {
    let path = url.toString().split(',');
    this.imagePath = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + path[1]);
    this.userForm.value.imgProfile.setValue(path[1]);
    //this.userForm.get('imgProfile')?.setValue(path[1]);
  }

  //#endregion
}
