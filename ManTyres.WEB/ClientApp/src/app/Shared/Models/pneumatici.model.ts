import { Data } from "@angular/router";
import { Depositi } from "./depositi.model";
import { Stagioni } from "./stagioni.model";
import { Veicolo } from "./veicoli.mdel";

export class Pneumatici {
  constructor(
    public pneumaticiId: number,
    public marca: string,
    public modello: string,
    public misura: string,
    public dot: string,
    public stagioneId: number | null,
    public veicoloId: number | null,
    public dataUbicazione: string,
    public quantita: number,
    public isDeleted: boolean,

    public stagione: Stagioni | null,
    public veicolo: Veicolo | null
  ) { }
}
