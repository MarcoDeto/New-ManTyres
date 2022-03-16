export class ChartDTO {
  constructor(
    public date: string,
    public value: number
  ) { }
}

export class Report {
  constructor(
    public label: string,
    public labels: string[],
    public Data: number[],
    public backgroundColor: string
  ) { }
}
