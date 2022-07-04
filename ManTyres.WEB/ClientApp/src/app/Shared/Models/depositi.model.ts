import { Sedi } from './sedi.model';

export class Depositi {
  constructor(
    public depositoId: number | null,
    public sedeId: number | null,
    public ubicazione: string | null,
    public isDeleted: boolean,
    public sede: Sedi | null
  ) { }
}
