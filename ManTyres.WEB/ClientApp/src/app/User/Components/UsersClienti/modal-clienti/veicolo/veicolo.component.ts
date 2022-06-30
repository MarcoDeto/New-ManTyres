import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { Mode } from '../../../../../Shared/Models/mode.model';
import { Veicolo } from '../../../../../Shared/Models/veicoli.mdel';
import { ModalVeicoliComponent } from '../../../UsersVeicoli/modal-veicoli/modal-veicoli.component';

@Component({
  selector: 'veicolo',
  templateUrl: './veicolo.component.html',
  styleUrls: ['./veicolo.component.scss']
})
export class VeicoloComponent implements OnInit {
  @Input()
  veicolo: Veicolo;
  hover = false;

  constructor(
    public dialog: MatDialog,
  ) { }

  ngOnInit(): void {

  }

  infoVeicolo(data: Veicolo) {
    console.log(data);
    const dialogRef = this.dialog.open(ModalVeicoliComponent, {
      width: '80%',
      data: { mode: Mode.View, veicolo: data }
    });

    dialogRef.beforeClosed().subscribe(result => { this.ngOnInit(); });
    dialogRef.afterClosed().subscribe(result => { this.ngOnInit(); });
  }

}
