import { Cliente } from '../../Shared/Models/clienti.model';

export class Veicolo {
  constructor(
    public veicoloId: number,
    public targa: string | null,
    public marca: string | null,
    public modello: string | null,
    public clienteId: number | null,
    public isDeleted: boolean,
    public cliente: Cliente | null
  ) { }
}

