import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Response } from 'src/app/Shared/Models/response.model';
import { Inventario } from 'src/app/Shared/Models/inventario.model';
import { ActivatedRoute } from '@angular/router';
import { PneumaticiService } from 'src/app/User/Services/pneumatici.service';
import { Mode } from 'src/app/Shared/Models/mode.model';
import { ModalPneumaticiComponent } from '../modal-pneumatici/modal-pneumatici.component';
import { FineDepositoComponent } from '../fine-deposito/fine-deposito.component';

@Component({
    selector: 'card-pneumatici',
    templateUrl: './card-pneumatici.component.html',
    styleUrls: ['./card-pneumatici.component.scss']
})

export class CardPneumaticiComponent implements OnInit, OnDestroy {
  @Input()
  inventario: Inventario | undefined = undefined;
  @Input()
  isVeicolo: boolean | null = null;

  subscribers: Subscription[];
  hover = false;

  constructor(
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private pneumaticiService: PneumaticiService,
    private toastr: ToastrService,
  ) {
    this.subscribers = [];
  }

  ngOnInit() {
  }

  hoverListItem(hover: boolean) {
    this.hover = !this.hover;
  }

  editPneumatici(data: Inventario) {
    Swal.fire({
      icon: 'question',
      title: 'Cosa vuoi fare?',
      showDenyButton: true,
      denyButtonText: 'Fine Deposito',
      confirmButtonText: 'Modifica'
    }).then((result) => {
      if (result.isConfirmed) {
        console.log(data);
        const dialogRef = this.dialog.open(ModalPneumaticiComponent, {
          maxWidth: '90%',
          data: { mode: Mode.Edit, inventario: data }
        });
        dialogRef.beforeClosed().subscribe(result => {
          if (result != undefined || result != null) {
            this.inventario = result;
          }
          this.ngOnInit();
        });
        dialogRef.afterClosed().subscribe(result => {
          if (result != undefined || result != null) {
            this.inventario = result;
          }
          this.ngOnInit();
        });
      }
      if (result.isDenied) {
        const dialogRef = this.dialog.open(FineDepositoComponent, {
          maxWidth: '90%',
          data: { mode: Mode.Edit, inventario: data }
        });
        dialogRef.beforeClosed().subscribe(result => {
          this.ngOnInit();
        });
        dialogRef.afterClosed().subscribe(result => {
          this.ngOnInit();
        });
      }
    });
  }

  deactivatePneumatici(id: number) {
    Swal.fire({
      title: 'Confermare la cancellazione?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    })
      .then((result) => {
        if (result.value) {
          this.subscribers.push(this.pneumaticiService.deactivatePneumatici(id).subscribe(
            (result: Response) => {
              this.inventario = result.content;
              this.toastr.success('Veicolo eliminato con successo!');
            }
          ));
        }
      });
  }

  reactivatePneumatici(data: Inventario) {
    Swal.fire({
      title: 'Ripristinare pneumatici?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    })
      .then((result) => {
        if (result.value) {
          data.isDeleted = false;
          this.subscribers.push(this.pneumaticiService.editPneumatici(data).subscribe(
            (result: Response) => {
              this.inventario = result.content;
              this.toastr.success('pneumatici riattivati con successo!');
            }
          ));
        }
      });
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

}
