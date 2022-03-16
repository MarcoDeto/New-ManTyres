export class Sedi {
  constructor(
    public sedeId: number,
    public nazione: string | null,
    public provincia: string | null,
    public cap: string | null,
    public comune: string | null,
    public indirizzo: string | null,
    public civico: string | null,
    public telefono: string | null,
    public isDeleted: boolean
  ) { }
}
