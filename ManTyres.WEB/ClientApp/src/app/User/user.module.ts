import { NgModule } from '@angular/core';
import { SharedModule } from '../Shared/shared.module';
import { AuthenticationModule } from '../Authentication/authentication.module';
import { UserRoutingModule } from './user-routing.module';
import { UserHomeComponent } from './Components/Pneumatici/user-home.component';
import { ClientiComponent } from './Components/UsersClienti/clienti/clienti.component';
import { ModalClientiComponent } from './Components/UsersClienti/modal-clienti/modal-clienti.component';
import { VeicoliComponent } from './Components/UsersVeicoli/veicoli/veicoli.component';
import { ModalVeicoliComponent } from './Components/UsersVeicoli/modal-veicoli/modal-veicoli.component';
import { ModalPneumaticiComponent } from './Components/Pneumatici/modal-pneumatici/modal-pneumatici.component';
import { DashboardComponent } from './Components/Dashboard/dashboard.component';
import { InventarioComponent } from './Components/inventario/inventario.component';
import { FineDepositoComponent } from './Components/Pneumatici/fine-deposito/fine-deposito.component';
import { InizioDepositoComponent } from './Components/Pneumatici/inizio-deposito/inizio-deposito.component';
import { VeicoloComponent } from './Components/UsersClienti/modal-clienti/veicolo/veicolo.component';

@NgModule({
  declarations: [
    UserHomeComponent,
    ClientiComponent,
    ModalClientiComponent,
    VeicoliComponent,
    ModalVeicoliComponent,
    ModalPneumaticiComponent,
    DashboardComponent,
    InventarioComponent,
    FineDepositoComponent,
    InizioDepositoComponent,
    VeicoloComponent
  ],
  imports: [
    UserRoutingModule,
    SharedModule,
    AuthenticationModule,
  ],
  entryComponents: [ModalClientiComponent, ModalVeicoliComponent, ModalPneumaticiComponent]
})
export class UserModule { }
