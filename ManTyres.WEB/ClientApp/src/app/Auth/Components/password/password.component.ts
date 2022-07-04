import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, Validators, Form } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { UserService } from '../../Services/user.service';
import { Response } from '../../../Shared/Models/response.model';
import { UserPassword } from '../../../Shared/Models/user.model';
import { ValidatorsService } from 'src/app/Shared/Validators/validators.services';

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
  selector: 'app-password',
  templateUrl: './password.component.html',
  styleUrls: ['./password.component.scss']
})
export class PasswordComponent implements OnInit, OnDestroy {
  title = 'Password';

  subscribers: Subscription[];

  passwordForm: FormGroup = this.formBuilder.group({});
  get getPasswordControl() { return this.passwordForm.controls; }
  newPasswordForm: FormGroup = this.formBuilder.group({});
  get getNewPasswordControl() { return this.newPasswordForm.controls; }

  userPassword: UserPassword = {
    id: null,
    userName: null,
    password: null,
    newPassword: null
  }
  caricamento = true;
  hide = true;
  passChecked = false;
  currentPassword = '';
  matcher = new ErrorPasswordStateMatcher();

  isPasswordEditMode: boolean = false;

  constructor(
    private userService: UserService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private validators: ValidatorsService,
    private formBuilder: FormBuilder,
  ) {
    this.subscribers = [];
  }

  ngOnInit(): void {
    this.userPassword.id = this.userService.getUserID();
    this.userPassword.userName = this.userService.getUsername();
    console.log('USERNAME = ' + this.userPassword.userName);
    this.caricamento = false;
    this.passwordForm = this.formBuilder.group({
      password: ['', [Validators.required]],
    });
    this.newPasswordForm = this.formBuilder.group({
      newPassword: ['', [Validators.required, Validators.minLength(8), this.validators.password()]],
      confermaPassword: ['', [Validators.required, Validators.minLength(8), this.validators.password()]]
    }, { validators: this.matchPasswords.bind(this) })

  }

  checkUserPassword() {
    this.caricamento = true;

    this.userPassword.password = this.passwordForm.value.password;
    this.userPassword.newPassword = this.passwordForm.value.password;

    this.subscribers.push(this.userService.checkCurrentPassword(this.userPassword).subscribe(
      (res: Response) => {
        this.caricamento = false;
        if (res.content) {
          this.toastr.success('Password corretta');
          this.passChecked = true;
        }
        else {
          this.toastr.error('Password errata');
          this.passChecked = false;

        }
      },
      err => {
        this.caricamento = false;
      }
    ));
  }

  changeValue() { this.hide = !this.hide; }

  changePassword() {
    this.caricamento = true;

    this.userPassword.newPassword = this.newPasswordForm.value.confermaPassword;

    this.subscribers.push(this.userService.changePassword(this.userPassword).subscribe(
      (res: Response) => {
        this.caricamento = false;
        if (res.content) {
          this.toastr.success('Password cambiata');
          this.router.navigate(['account/profile']);
        }
        else {
          this.toastr.error(res.message);
        }
      },
      err => {
        this.caricamento = false;
      }
    ));
  }

  matchPasswords(group: FormGroup) {
    const password = group.value.newPassword;
    const confirmPassword = group.value.confermaPassword;

    if (confirmPassword != '')
      return password === confirmPassword ? null : { notSame: true }
    
    return null;
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
