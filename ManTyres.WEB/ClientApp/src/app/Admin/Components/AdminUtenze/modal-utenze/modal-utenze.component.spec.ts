import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalUtenzeComponent } from './modal-utenze.component';

describe('ModalUtenzeComponent', () => {
  let component: ModalUtenzeComponent;
  let fixture: ComponentFixture<ModalUtenzeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalUtenzeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalUtenzeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
