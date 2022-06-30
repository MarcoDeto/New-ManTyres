import { NgModule } from '@angular/core';
import { SharedModule } from '../Shared/shared.module';

import { AdminHomeComponent } from './Components/AdminHome/admin-home.component';
import { AdminDatabaseComponent } from './Components/AdminDatabase/admin-database.component';
import { ModalUtenzeComponent } from './Components/AdminUtenze/modal-utenze/modal-utenze.component';
import { SedeDetailComponent } from './Components/AdminSedi/sede-detail/sede-detail.component';
import { CardSedeComponent } from './Components/AdminSedi/card-sede/card-sede.component';
import { UtenzeDetailComponent } from './Components/AdminUtenze/utenze-detail/utenze-detail.component';
import { RouterModule } from '@angular/router';
import { UtenzeComponent } from './Components/AdminUtenze/Utenze/utenze.component';
import { SediComponent } from './Components/AdminSedi/Sedi/sedi.component';


@NgModule({
  declarations: [
    AdminHomeComponent,
    AdminDatabaseComponent,
    ModalUtenzeComponent,
    SediComponent,
    SedeDetailComponent,
    CardSedeComponent,
    UtenzeDetailComponent,
    UtenzeComponent,
  ],
  imports: [
    RouterModule.forRoot([
      {
        path: 'admin',
        children: [
          { path: 'home', component: AdminHomeComponent },
          { path: 'users', component: UtenzeComponent },
          { path: 'user/:userId', component: UtenzeDetailComponent },
          { path: 'sedi', component: SediComponent },
          { path: 'sede/:sedeId', component: SedeDetailComponent },
          { path: 'insertdb', component: AdminDatabaseComponent },
        ],
      },
      { path: 'login', redirectTo: 'account' },
    ]),
    SharedModule,
  ],
  exports: [
  ],
  entryComponents: [
    ModalUtenzeComponent
  ],
})
export class AdminModule { }
/*
const adminRoutes: Routes = [
  { path: 'admin/home', component: AdminHomeComponent, canActivate: [AuthGuard], data: { permittedRoles: ['admin'] } },
  { path: 'admin/users', component: UtenzeComponent, canActivate: [AuthGuard], data: { permittedRoles: ['admin'] } },
  { path: 'admin/user/:userId', component: UtenzeDetailComponent, canActivate: [AuthGuard], data: { permittedRoles: ['admin'] } },
  { path: 'admin/sedi', component: SediComponent, canActivate: [AuthGuard], data: { permittedRoles: ['admin'] } },
  { path: 'admin/sede/:sedeId', component: SedeDetailComponent, canActivate: [AuthGuard], data: { permittedRoles: ['admin'] } },
  { path: 'admin/insertdb', component: AdminDatabaseComponent, canActivate: [AuthGuard], data: { permittedRoles: ['admin'] } },
  { path: '**', component: NotFoundComponent }
];*/