import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UtenzeComponent } from './utenze.component';

describe('UtenzeComponent', () => {
  let component: UtenzeComponent;
  let fixture: ComponentFixture<UtenzeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [UtenzeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UtenzeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
