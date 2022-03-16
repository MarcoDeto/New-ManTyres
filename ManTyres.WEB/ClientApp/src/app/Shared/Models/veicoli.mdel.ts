import { Cliente } from '../../Shared/Models/clienti.model';

export class Veicolo {
  constructor(
    public veicoloId: number,
    public targa: string,
    public marca: string | null,
    public modello: string | null,
    public clienteId: number,
    public isDeleted: boolean,
    public cliente: Cliente | null
  ) { }
}

