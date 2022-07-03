import { Component, Renderer2 } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { UserService } from './Auth/Services/user.service';
import { ConnectionInfoService } from './Shared/Services/connectionInfo.service';
import { LoaderService } from './Shared/Services/loader.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ManTyres';

  opened: boolean = false;
  caricamento: boolean = false;

  constructor(
    private titleService: Title,
    private loaderService: LoaderService,
    private renderer: Renderer2,
    public userService: UserService,
    private connService: ConnectionInfoService,
  ) { }

  ngAfterViewInit() {
    this.loaderService.httpProgress().subscribe((status: boolean) => {
      if (status) {
        //this.caricamento = true;
        this.renderer.addClass(document.body, 'cursor-loader');
      } else {
        //this.caricamento = false;
        this.renderer.removeClass(document.body, 'cursor-loader');
      }
    });
  }

  onRouterOutletActivate(event: any) {
    if (!event || !event.route || !event.route.url || 
      !event.route.url._value || !event.route.url._value[0] 
      || !event.title) {
        this.titleService.setTitle(this.title);
      return;
    }
    this.titleService.setTitle(`${this.title} - ${event.title}`);
  }

  ngOnInit(): void {
    if (this.getWidth() > 800) {
      this.opened = false;
    }
    this.connService.setLangCurrency();
  }

  getWidth(): number { return window.innerWidth; }

}