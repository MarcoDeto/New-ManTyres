import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardSedeComponent } from './card-sede.component';

describe('CardSedeComponent', () => {
  let component: CardSedeComponent;
  let fixture: ComponentFixture<CardSedeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CardSedeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CardSedeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
