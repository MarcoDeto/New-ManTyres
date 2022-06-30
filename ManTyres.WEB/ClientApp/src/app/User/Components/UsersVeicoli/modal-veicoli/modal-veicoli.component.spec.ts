import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ModalVeicoliComponent } from './modal-veicoli.component';

let component: ModalVeicoliComponent;
let fixture: ComponentFixture<ModalVeicoliComponent>;

describe('ModalVeicoli component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ModalVeicoliComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ModalVeicoliComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});
