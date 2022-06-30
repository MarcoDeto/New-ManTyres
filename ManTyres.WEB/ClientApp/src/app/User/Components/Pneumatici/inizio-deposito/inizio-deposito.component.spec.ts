import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InizioDepositoComponent } from './inizio-deposito.component';

describe('InizioDepositoComponent', () => {
  let component: InizioDepositoComponent;
  let fixture: ComponentFixture<InizioDepositoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InizioDepositoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InizioDepositoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
