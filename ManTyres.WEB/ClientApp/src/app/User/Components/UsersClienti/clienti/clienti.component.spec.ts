import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ClientiComponent } from './clienti.component';

let component: ClientiComponent;
let fixture: ComponentFixture<ClientiComponent>;

describe('Clienti component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ClientiComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ClientiComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});
