import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from "@angular/core";
import { Subscription } from "rxjs";
import { Sedi } from "../../../Shared/Models/sedi.model";
import { SediService } from "../../Services/sedi.service";
import { AdminDatabaseService } from "../../Services/admin.database.service";
import { Response } from 'src/app/Shared/Models/response.model';
import { FormBuilder, FormGroup } from "@angular/forms";
import { saveAs } from 'file-saver';
import { ProgressStatus } from "../../../Shared/Models/progressStatus.model";
import { ToastrService } from "ngx-toastr";
import { UserService } from "src/app/Auth/Services/user.service";

@Component({
  selector: 'app-admin-database',
  templateUrl: './admin-database.component.html',
  styleUrls: ['./admin-database.component.scss']
})

export class AdminDatabaseComponent implements OnInit, OnDestroy{
  title = 'Import Excel';
  //old
  @ViewChild('input', {static: true}) inputRef : ElementRef | null = null;
  @ViewChild('label', { static: true}) labelRef: ElementRef | null = null;
  //new
  @Input() public disabled: boolean = false;
  @Output() public uploadStatus: EventEmitter<ProgressStatus> | null = null;
  @ViewChild('inputFile') inputFile: ElementRef | null = null;

  nomeFile: any;
  all: any;
  clienti: any;
  veicoli: any;
  subscribers: Subscription[] = [];
  sedi: Sedi[] = [];
  importForm: FormGroup = this.formBuilder.group({});
  loading: boolean = false;

  constructor(
    private sediService: SediService,
    private dbService: AdminDatabaseService,
    public userService: UserService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService
  ) 
  { }

  ngOnInit(): void {
    this.getSedi();
    this.importForm = this.formBuilder.group({
      sede: 0
    });
  }

  getSedi(): void {
    this.subscribers.push(this.sediService.getAllSedi().subscribe(
      (res: Response) => {
        this.sedi = res.content;
      },
      err => {
        this.sedi.length = 0;
        this.sedi.splice(0);
      }
    ));
  }

  tracciatoClienti(): void {
    this.subscribers.push(this.dbService.tracciatoClienti().subscribe(
      res => {
        const blob = new Blob([res]);
        saveAs(blob, "Tracciato clienti.xlsx");
      }
    ));
  }

  tracciatoVeicoli(): void {
    this.subscribers.push(this.dbService.tracciatoVeicoli().subscribe(
      res => {
        const blob = new Blob([res]);
        saveAs(blob, "Tracciato veicoli.xlsx");
      }
    ));
  }

  tracciatoPneumatici(): void {
    this.subscribers.push(this.dbService.tracciatoPneumatici().subscribe(
      res => {
        const blob = new Blob([res]);
        saveAs(blob, "Tracciato completo.xlsx");
      }
    ));
  }

  loadAll(event: any) {
    this.all = event.target.files[0];
    if (this.all) {
      let path = this.inputRef?.nativeElement.value;
      let name = path.split('\\')[2];
      this.labelRef!.nativeElement.innerHTML = name;
    }
  }

  loadClienti(event: any) {
    this.clienti = event.target.files[0];
    if (this.clienti) {
      let path = this.inputRef?.nativeElement.value;
      let name = path.split('\\')[2];
      this.labelRef!.nativeElement.innerHTML = name;
    }
  }

  loadVeicoli(event: any) {
    this.veicoli = event.target.files[0];
    if (this.veicoli) {
      let path = this.inputRef?.nativeElement.value;
      let name = path.split('\\')[2];
      this.labelRef!.nativeElement.innerHTML = name;
    }
  }

  caricaTutto() {
    this.loading = true;
    let sedeId = this.importForm.value.sede;
    let userId = this.userService.getUserID();
    this.subscribers.push(this.dbService.caricaTutto(this.all, sedeId, userId!, navigator.language).subscribe(
      res => {
        this.toastr.success(res.message);
        this.loading = false;
      },
      err => {
        this.loading = false;
      }
    ));
  }

  caricaClienti() {
    this.subscribers.push(this.dbService.caricaClienti(this.clienti).subscribe(
      res => console.log(res)
    ));
  }

  caricaVeicoli() {
    this.subscribers.push(this.dbService.caricaVeicoli(this.veicoli).subscribe(
      res => console.log(res)
    ));
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }
}
