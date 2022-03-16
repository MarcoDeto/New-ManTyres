import { Sedi } from "./sedi.model";

export class Depositi {
  constructor(
    public depositoId: number,
    public sedeId: number,
    public ubicazione: string,
    public isDeleted: boolean,
    public sede: Sedi
  ) { }
}
