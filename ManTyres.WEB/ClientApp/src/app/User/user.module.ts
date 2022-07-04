import { NgModule } from '@angular/core';
import { UserRoutingModule } from './user-routing.module';
import { UserHomeComponent } from './Components/home/user-home.component';
import { ClientiComponent } from './Components/clienti/clienti/clienti.component';
import { ModalClientiComponent } from './Components/clienti/modal-clienti/modal-clienti.component';
import { VeicoliComponent } from './Components/veicoli/veicoli/veicoli.component';
import { ModalVeicoliComponent } from './Components/veicoli/modal-veicoli/modal-veicoli.component';
import { ModalPneumaticiComponent } from './Components/pneumatici/modal-pneumatici/modal-pneumatici.component';
import { DashboardComponent } from './Components/dashboard/dashboard.component';
import { InventarioComponent } from './Components/inventario/inventario.component';
import { FineDepositoComponent } from './Components/pneumatici/fine-deposito/fine-deposito.component';
import { InizioDepositoComponent } from './Components/pneumatici/inizio-deposito/inizio-deposito.component';
import { VeicoloComponent } from './Components/clienti/modal-clienti/veicolo/veicolo.component';
import { SharedModule } from '../Shared/shared.module';
import { CardPneumaticiComponent } from './Components/pneumatici/card-pneumatici/card-pneumatici.component';

@NgModule({
  declarations: [
    UserHomeComponent,
    ClientiComponent,
    VeicoliComponent,
    ModalPneumaticiComponent,
    ModalVeicoliComponent,
    ModalClientiComponent,
    CardPneumaticiComponent,
    DashboardComponent,
    InventarioComponent,
    FineDepositoComponent,
    InizioDepositoComponent,
    VeicoloComponent
  ],
  imports: [
    UserRoutingModule,
    SharedModule
  ],
  entryComponents: []
})
export class UserModule { }
