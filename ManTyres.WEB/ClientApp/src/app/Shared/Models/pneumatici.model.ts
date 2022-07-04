import { Data } from '@angular/router';
import { Depositi } from './depositi.model';
import { Stagioni } from './stagioni.model';
import { Veicolo } from './veicoli.mdel';

export class Pneumatici {
  constructor(
    public pneumaticiId: number,
    public marca: string | null,
    public modello: string | null,
    public misura: string | null,
    public dot: string | null,
    public stagioneId: number | null,
    public veicoloId: number | null,
    public dataUbicazione: string | null,
    public quantita: number,
    public isDeleted: boolean,

    public stagione: Stagioni | null,
    public veicolo: Veicolo | null
  ) { }
}
