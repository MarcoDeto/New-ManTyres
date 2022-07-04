import { Cliente } from '../../Shared/Models/clienti.model';
import { Depositi } from './depositi.model';
import { Pneumatici } from './pneumatici.model';
import { Stagioni } from './stagioni.model';
import { User } from './user.model';

export class Inventario {
  constructor(
    public pneumaticiId: number,
    public inizioDeposito: string | null,
    public fineDeposito: string | null,
    public depositoId: number | null,
    public battistrada: number | null,
    public statoGomme: string | null,
    public userId: string | null,
    public isDeleted: boolean | null,
    public pneumatici: Pneumatici | null,
    public deposito: Depositi | null,
    public user: User | null
  ) { }
}
