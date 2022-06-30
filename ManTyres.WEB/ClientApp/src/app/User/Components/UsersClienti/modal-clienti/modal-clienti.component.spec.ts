import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ModalClientiComponent } from './modal-clienti.component';

let component: ModalClientiComponent;
let fixture: ComponentFixture<ModalClientiComponent>;

describe('Modal-Clienti component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ModalClientiComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ModalClientiComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});
