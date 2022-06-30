import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserHomeComponent } from './Components/Pneumatici/user-home.component';
import { AuthGuard } from '../Authentication/auth.guard';
import { VeicoliComponent } from './Components/UsersVeicoli/veicoli/veicoli.component';
import { ClientiComponent } from './Components/UsersClienti/clienti/clienti.component';
import { DashboardComponent } from './Components/Dashboard/dashboard.component';
import { InventarioComponent } from './Components/inventario/inventario.component';

const userRoutes: Routes = [
  { path: 'pneumatici', component: UserHomeComponent, canActivate: [AuthGuard], data: { permittedRoles: ['user', 'admin'] } },
  { path: 'clienti', component: ClientiComponent, canActivate: [AuthGuard], data: { permittedRoles: ['user', 'admin'] } },
  { path: 'veicoli', component: VeicoliComponent, canActivate: [AuthGuard], data: { permittedRoles: ['user', 'admin'] } },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard], data: { permittedRoles: ['user', 'admin'] } },
  { path: 'inventario', component: InventarioComponent, canActivate: [AuthGuard], data: { permittedRoles: ['user', 'admin'] } }
];

@NgModule({
  imports: [RouterModule.forChild(userRoutes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
