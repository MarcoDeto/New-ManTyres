export class Cliente {
  constructor(
    public clienteId: number,
    public cognome: string | null,
    public nome: string | null,
    public codiceFiscale: string | null,
    public partitaIva: string | null,
    public nazione: string | null,
    public provincia: string | null,
    public cap: string | null,
    public comune: string | null,
    public indirizzo: string | null,
    public civico: string | null,
    public email: string | null,
    public telefono: string | null,
    public isAzienda: boolean,
    public isDeleted: boolean
  ) { }
}
export class SelectClienti {
  constructor(
    public clienteId: number,
    public cognome_Nome: string | null
  ) {}
}
