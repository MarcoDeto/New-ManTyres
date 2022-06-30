import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalPneumaticiComponent } from './modal-pneumatici.component';

describe('ModalPneumaticiComponent', () => {
  let component: ModalPneumaticiComponent;
  let fixture: ComponentFixture<ModalPneumaticiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalPneumaticiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalPneumaticiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
