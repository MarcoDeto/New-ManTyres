import { Component, OnDestroy, OnInit } from "@angular/core";
import { Subscription } from "rxjs";
import { User, UserPassword, UserName, Utenza } from "../../../Shared/Models/user.model";
import Swal from 'sweetalert2';
import { DomSanitizer } from '@angular/platform-browser';
import { FormGroup, Validators, FormBuilder} from "@angular/forms";
import { ToastrService } from "ngx-toastr";
import { ActivatedRoute, Router } from "@angular/router";
import { ValidatorsService } from "../../../Shared/Validators/validators.services";
import { UserService } from "../../Services/user.service";
import { AdminUserService } from "../Services/admin-user.service";
import { Response } from "../../../Shared/Models/response.model";

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
  user: Utenza = { id: '', email: '', emailConfirmed: false, firstName: '', imgProfile: '', isDeleted: false, lastName: '', phoneNumber: '', userName: '', confirmPassword: '', password: '', role: '' };
  caricamento = true;
  hide = true;

  currentUserName: string | undefined;

  constructor(
    private sanitizer: DomSanitizer,
    private userService: UserService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private validators: ValidatorsService,
    private formBuilder: FormBuilder,
    private adminUserService: AdminUserService
  ) { }

  ngOnInit(): void {
    
    if (this.userService.getUsername() == null) { return; }

    this.currentUserName = this.userService.getUsername()!;
    this.subscribers.push(this.userService.profile(this.currentUserName).subscribe(
      (res: Response) => {
        this.user = res.content;
        this.caricamento = false;
        this.userForm = this.formBuilder.group({
          id: [this.user.id],
          imgProfile: [this.user.imgProfile],
          firstname: [this.user.firstName, [Validators.required, Validators.maxLength(50), this.validators.fullName()]],
          lastname: [this.user.lastName, [Validators.required, Validators.maxLength(50), this.validators.fullName()]],
          username: [this.user.userName, [Validators.required, this.validators.userName()]],
          email: [this.user.email, [Validators.required, Validators.email]],
          phoneNumber: [this.user.phoneNumber, [Validators.required, Validators.maxLength(20)]],
          role: [this.user.role, [Validators.required]]
        });
        this.imagePath = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + this.userForm.value.imgProfile);
      },
      error => {
        console.log(error);
        this.caricamento = false;
      }
    ));
  }

  OnSubmit() {
    this.caricamento = true;

    this.subscribers.push(this.adminUserService.putUser(this.userForm.value).subscribe(
      (res: Response) => {
        this.caricamento = false;
        if (res.content) {
          this.toastr.success("Profilo modificato");
        }
        else {
          this.toastr.error(res.message);
        }
      },
      err => {
        this.caricamento = false;
        this.toastr.error("Errore!" + err.error);
        console.error(err.error.errorMessage);
      }
    ));

    if (this.currentUserName != this.userForm.value.username) {
      Swal.fire({
        title: "Hai cambiato username, rifare l'accesso!",
        icon: 'warning',
        confirmButtonText: 'Ok',
      }).then((result) => {
        this.userService.removeToken();
        this.userService.logout();
        this.router.navigate(['']);
        this.userService.no();
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
    return text.replace(/(?:\s+)\s/g, " ");
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
