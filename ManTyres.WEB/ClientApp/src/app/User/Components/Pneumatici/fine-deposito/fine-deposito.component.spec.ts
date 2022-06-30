import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FineDepositoComponent } from './fine-deposito.component';

describe('FineDepositoComponent', () => {
  let component: FineDepositoComponent;
  let fixture: ComponentFixture<FineDepositoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FineDepositoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FineDepositoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
