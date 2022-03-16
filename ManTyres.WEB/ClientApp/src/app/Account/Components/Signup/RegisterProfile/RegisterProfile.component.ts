import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-register-profile',
  templateUrl: './RegisterProfile.component.html',
  styleUrls: ['./RegisterProfile.component.scss']
})
export class RegisterProfileComponent implements OnInit {

  profileForm: FormGroup = this.formBuilder.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    phone: ['', Validators.required],
  });
  caricamento = false;
	get getProfileControl() { return this.profileForm.controls; }

  constructor(
    private formBuilder: FormBuilder,
    public translate: TranslateService,
    private router: Router,
  ) { }

  ngOnInit() {
  }

  onSubmit() {
    this.router.navigate(['account/register/address']);
    /* si ferma se il modulo non è valido
    if (this.signupForm.invalid) {
      return;
    }
    this.userService.setSession("test");
    */
  }
}
