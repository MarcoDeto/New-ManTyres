import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Inventario } from '../../../Shared/Models/inventario.model';
import { Role } from '../../../Shared/Models/role.model';
import { DashboardService } from '../../Services/dashboard.service';
import { Response } from 'src/app/Shared/Models/response.model';
import * as Chart from 'chart.js';
import { Report } from '../../../Shared/Models/chart.model';
// import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
// import { Color, BaseChartDirective, Label } from 'ng2-charts';
// import * as pluginAnnotations from 'chartjs-plugin-annotation';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})

export class DashboardComponent {
  title = 'Dashboard';

  subscribers: Subscription[] = [];
  caricamento = false;
  notFound = false;
  dbEmpty = false;

  Pneumatici = 0;
  Veicoli = 0;
  Clienti = 0;
  Utenti = 0;

  label = 'Depositi';
  backgroundColor = 'rgba(0, 123, 255,0.3)';
  borderColor = 'rgb(0, 123, 255)';
  /*
  @ViewChild(BaseChartDirective) chart: BaseChartDirective | undefined;
  lineChart: ChartType = 'line';
  barChart: ChartType = 'bar';


  lineChartOptions: ChartOptions = {
    responsive: true,
    scales: {
      yAxes: [{
        ticks: {
          beginAtZero: true
        }
      }]
    }
  };

  chartLabels: string[] = [];
  chartData = [];
  lineChartData: ChartDataSets[] = [
    { data: [], label: '' }
  ];
  lineChartLabels: Label[] = [];
  lineChartColors: Color[] = [
    { // red
      backgroundColor: this.backgroundColor,
      borderColor: this.borderColor,
    }
  ];
  lineChartLegend = true;
  lineChartType = this.lineChart;
  lineChartPlugins = [pluginAnnotations];
  */

  month = 1;
  pneumatici = true;
  veicoli = false;
  clienti = false;
  globale = false;
  quantita = true;


  constructor(
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private http: HttpClient,
    public dialog: MatDialog,
    private dashboardService: DashboardService,
    private toastr: ToastrService,
  ) {
    this.subscribers = [];
  }

  ngOnInit() {
    this.getPneumatici();
    this.getVeicoli();
    this.getClienti();
    this.getUtenti();

    //this.GetChart(this.month);
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

  getWidth(): number {
    return window.innerWidth;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Event) { this.ngOnInit(); }

  getPneumatici() {
    this.subscribers.push(this.dashboardService.Pneumatici().subscribe(
      (res: Response) => {
        if (res) {
          this.Pneumatici = res.content;
        }
      }
    ));
  }

  getVeicoli() {
    this.subscribers.push(this.dashboardService.Veicoli().subscribe(
      (res: Response) => {
        if (res) {
          this.Veicoli = res.content;
        }
      }
    ));
  }

  getClienti() {
    this.subscribers.push(this.dashboardService.Clienti().subscribe(
      (res: Response) => {
        if (res) {
          this.Clienti = res.content;
        }
      }
    ));
  }

  getUtenti() {
    this.subscribers.push(this.dashboardService.Utenti().subscribe(
      (res: Response) => {
        if (res) {
          this.Utenti = res.content;
        }
      }
    ));
  }

  /*
  CreatePneumatici() {
    this.pneumatici = true;
    this.veicoli = false;
    this.clienti = false;
    this.GetChart(this.month);
  }

  CreateVeicoli() {
    this.pneumatici = false;
    this.veicoli = true;
    this.clienti = false;
    this.GetChart(this.month);
  }

  CreateClienti() {
    this.pneumatici = false;
    this.veicoli = false;
    this.clienti = true;
    this.GetChart(this.month);
  }

  CreateGlobale() {
    this.globale = true;
    this.quantita = false;
    this.GetChart(this.month);
  }

  CreateGiornaliero() {
    this.globale = false;
    this.quantita = true;
    this.GetChart(this.month);
  }

  GetChart(month: number) {
    if (this.globale)
      this.lineChartType = this.lineChart;
    else
      this.lineChartType = this.barChart;

    this.month = month;
    if (this.pneumatici) {
      this.backgroundColor = 'rgba(0, 123, 255,0.4)';
      this.borderColor = 'rgb(0, 123, 255)';
      if (this.globale) {
        this.label = 'Totale depositi';
        this.ChartGlobalePneumatici(this.month);
      }
      else if (this.quantita) {
        this.label = 'Depositi';
        this.ChartGiornalieroPneumatici(this.month);
      }
    }
    else if (this.veicoli) {
      this.backgroundColor = 'rgba(255, 193, 7,0.4)';
      this.borderColor = 'rgb(255, 193, 7)';
      if (this.globale) {
        this.label = 'Totale veicoli';
        this.ChartGlobaleVeicoli(this.month);
      }
      else if (this.quantita) {
        this.label = 'Nuovi veicoli';
        this.ChartGiornalieroVeicoli(this.month);
      }
    }
    else if (this.clienti) {
      this.backgroundColor = 'rgba(40, 167, 69,0.4)';
      this.borderColor = 'rgb(40, 167, 69)';
      if (this.globale) {
        this.label = 'Totale clienti';
        this.ChartGlobaleClienti(this.month);
      }
      else if (this.quantita) {
        this.label = 'Nuovi clienti';
        this.ChartGiornalieroClienti(this.month);
      }
    }
  }

  ChartGlobalePneumatici(month: number) {
    this.subscribers.push(this.dashboardService.ChartGlobalePneumatici(month, navigator.language).subscribe(
      (res: Response) => {
        console.log(res);
        this.createChart(res);
      }
    ));
  }

  ChartGiornalieroPneumatici(month: number) {
    this.subscribers.push(this.dashboardService.ChartGiornalieroPneumatici(month, navigator.language).subscribe(
      (res: Response) => {
        console.log(res);
        this.createChart(res);
      }
    ));
  }

  ChartGlobaleVeicoli(month: number) {
    this.subscribers.push(this.dashboardService.ChartGlobaleVeicoli(month, navigator.language).subscribe(
      (res: Response) => {
        console.log(res);
        this.createChart(res);
      }
    ));
  }

  ChartGiornalieroVeicoli(month: number) {
    this.subscribers.push(this.dashboardService.ChartGiornalieroVeicoli(month, navigator.language).subscribe(
      (res: Response) => {
        console.log(res);
        this.createChart(res);
      }
    ));
  }

  ChartGlobaleClienti(month: number) {
    this.subscribers.push(this.dashboardService.ChartGlobaleClienti(month, navigator.language).subscribe(
      (res: Response) => {
        console.log(res);
        this.createChart(res);
      }
    ));
  }

  ChartGiornalieroClienti(month: number) {
    this.subscribers.push(this.dashboardService.ChartGiornalieroClienti(month, navigator.language).subscribe(
      (res: Response) => {
        console.log(res);
        this.createChart(res);
      }
    ));
  }

  createChart(res: Response) {
    setTimeout(() => {
      this.lineChartData = [
        { data: [], label: this.label }
      ];
      this.lineChartLabels.length = 0;
      this.lineChartData[0].data.length = 0;
      for (var i = 0; i < res.content.length; i++) {
        this.lineChartLabels.push(res.content[i].date);
        this.lineChartData[0].data.push(res.content[i].value);
      };
      this.lineChartColors = [
        {
          backgroundColor: this.backgroundColor,
          borderColor: this.borderColor,
        }
      ];
      this.chart.chart.update()
    }, 10);
  }
*/
}
