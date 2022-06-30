import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { VeicoliComponent } from './veicoli.component';

let component: VeicoliComponent;
let fixture: ComponentFixture<VeicoliComponent>;

describe('Veicoli component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ VeicoliComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(VeicoliComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});
