import { Component, OnDestroy, OnInit } from '@angular/core';
import { Form, FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import { Sedi } from '../../Models/sedi.model';
import { Utenza } from '../../Models/user.model';
import { ValidatorsService } from '../../Validators/validators.services';
import { Response } from '../../../Shared/Models/response.model'
import { FirstConnectionService } from '../../Services/FirstConnection.service';

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
  selector: 'app-setup',
  templateUrl: './setup.component.html',
  styleUrls: ['./setup.component.scss']
})

export class SetupComponent implements OnInit, OnDestroy{

  title = 'SETUP';

  desktop = true;
  caricamento = true;
  admin = false;
  firstsede = false;
  hide = true;
  matcher = new ErrorPasswordStateMatcher();
  subscribers: Subscription[];
  creationUserForm: FormGroup = this.formBuilder.group({});
  creationSedeForm: FormGroup = this.formBuilder.group({});
  user: Utenza = {
    confirmPassword: null,
    email: null,
    emailConfirmed: false,
    firstName: null,
    id: null,
    imgProfile: null,
    isDeleted: false,
    lastName: null,
    password: null,
    phoneNumber: null,
    role: null,
    userName: null
  };
  sede: Sedi = {
    cap: null,
    civico: null,
    comune: null,
    indirizzo: null,
    isDeleted: false,
    nazione: null,
    provincia: null,
    sedeId: 0,
    telefono: null,
    email: null,
  };


  constructor(
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private validators: ValidatorsService,
    private toastr: ToastrService,
    private setupService: FirstConnectionService,
  ) {
    this.subscribers = [];
    /*this.subscribers.push(this.setupService.GetSetup().subscribe(
      (res: Response) => {
        if (res.content.utenti != 0)
          this.admin = true;

        if (res.content.sedi != 0)
          this.firstsede = true;

        this.caricamento = false;
      },
      err => {
        this.router.navigate(['errore']);
        this.caricamento = false;
      }
    ));*/
  }

  ngOnInit(): void {
    
    if (this.getWidth() < 800) {
      this.desktop = false;
    }
    this.creationUserForm = this.formBuilder.group({
      imgProfile: [''],
      firstname: ['', [Validators.required, Validators.maxLength(50), this.validators.fullName()]],
      lastname: ['', [Validators.required, Validators.maxLength(50), this.validators.fullName()]],
      username: ['Admin', [Validators.required, this.validators.userName()]],
      email: ['', [Validators.required, Validators.email]],
      telefono: ['+39 ', [Validators.required, Validators.maxLength(20)]],
      password: ['', [Validators.required, Validators.minLength(8), this.validators.password()]],
      confermaPassword: ['', [Validators.required]],
      role: ['admin', Validators.required]
    }, { validators: this.checkPasswords.bind(this) });

    this.creationSedeForm = this.formBuilder.group({
      nazione: ['', [Validators.required, Validators.maxLength(100)]],
      provincia: ['', [Validators.required, Validators.maxLength(100)]],
      cap: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(5)]],
      comune: ['', [Validators.required, Validators.maxLength(100)]],
      indirizzo: ['', [Validators.required, Validators.maxLength(200)]],
      civico: ['', [Validators.required, Validators.maxLength(10)]],
      telefono: ['+39 ', [Validators.required, Validators.maxLength(20)]]
    });
  }

  changeValue(value: boolean) {
    this.hide = !value;
  } 

  checkPasswords(group: FormGroup) {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confermaPassword')?.value;

    if (confirmPassword != '')
      return password === confirmPassword ? null : { notSame: true }
    return null;
  }

  getWidth(): number {
    return window.innerWidth;
  }

  AddAdmin() {
    this.caricamento = true;
    this.user = {
      id: '',
      imgProfile: this.creationUserForm.value.imgProfile,
      firstName: this.replaceSpacesInOne(this.creationUserForm.value.firstname),
      lastName: this.replaceSpacesInOne(this.creationUserForm.value.lastname),
      userName: this.replaceSpacesInOne(this.creationUserForm.value.username),
      email: this.creationUserForm.value.email,
      emailConfirmed: false,
      phoneNumber: this.creationUserForm.value.telefono,
      password: this.creationUserForm.value.password,
      confirmPassword: this.creationUserForm.value.confermaPassword,
      role: 'admin',
      isDeleted: false
    };

    this.creationUserForm.reset();

    /*this.subscribers.push(this.setupService.AddAdmin(this.user).subscribe(
      (res: Response) => {
        this.caricamento = false;
        this.toastr.success('Creato Amministratore', 'Username: Admin');
        this.admin = true;
      },
      err => {
        this.caricamento = false;
      }
    ));*/
  }

  AddSede() {
    this.caricamento = true;
    this.sede = {
      sedeId: 0,
      nazione: this.creationSedeForm.value.nazione,
      provincia: this.creationSedeForm.value.provincia,
      cap: this.creationSedeForm.value.cap,
      comune: this.creationSedeForm.value.comune,
      indirizzo: this.creationSedeForm.value.indirizzo,
      civico: this.creationSedeForm.value.civico,
      telefono: this.creationSedeForm.value.telefono,
      email: this.creationSedeForm.value.email,
      isDeleted: false
    }

    this.creationSedeForm.reset();

    /*this.subscribers.push(this.setupService.AddFirstSede(this.sede).subscribe(
      (res: Response) => {
        this.caricamento = false;
        this.toastr.success('Sede aggiunta con successo!');
        this.firstsede = true;
      },
      err => {
        this.caricamento = false;
      }
    ));*/
  }

  replaceSpacesInOne(text: string) {
    if (!text) return text;
    text = text.charAt(0).toUpperCase() + text.substr(1).toLowerCase();
    return text.replace(/(?:\s+)\s/g, ' ');
  }

  routeHome() {
    this.router.navigate(['/account/login']);
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

  //#region metodi per image
  imageClicked: boolean = false;
  toImage = 'data:image/jpg;base64';
  imagePath: any | undefined;
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
        this.creationUserForm.value.imgProfile.setValue(null);
        //this.creationUserForm.get('imgProfile')?.setValue(null);
      }
    });
  }

  setImagePath(url: string) {
    let path = url.toString().split(',');
    this.imagePath = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + path[1]);
    this.creationUserForm.value.imgProfile.setValue(path[1]);
    //this.creationUserForm.get('imgProfile')?.setValue(path[1]);
  }

  //#endregion
}
