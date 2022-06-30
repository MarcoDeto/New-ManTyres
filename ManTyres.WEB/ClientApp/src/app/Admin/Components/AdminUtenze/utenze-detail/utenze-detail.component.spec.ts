import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UtenzeDetailComponent } from './utenze-detail.component';

describe('UtenzeDetailComponent', () => {
  let component: UtenzeDetailComponent;
  let fixture: ComponentFixture<UtenzeDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UtenzeDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UtenzeDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
