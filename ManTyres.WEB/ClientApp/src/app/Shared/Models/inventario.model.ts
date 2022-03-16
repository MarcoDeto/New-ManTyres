import { Cliente } from '../../Shared/Models/clienti.model';
import { Depositi } from './depositi.model';
import { Pneumatici } from './pneumatici.model';
import { Stagioni } from './stagioni.model';
import { User } from './user.model';

export class Inventario {
  constructor(
    public pneumaticiId: number,
    public inizioDeposito: string,
    public fineDeposito: string,
    public depositoId: number,
    public battistrada: number,
    public statoGomme: string,
    public userId: string,
    public isDeleted: boolean,
    public pneumatici: Pneumatici,
    public deposito: Depositi,
    public user: User
  ) { }
}
