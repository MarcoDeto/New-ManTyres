import { Component, Renderer2 } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { UserService } from './Auth/Services/user.service';
import { LoaderService } from './Shared/Services/loader.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'MANTYRES';

  opened: boolean = false;
  hideHeader: boolean = false;
  caricamento: boolean = false;

  constructor(
    private titleService: Title,
    private loaderService: LoaderService,
    private renderer: Renderer2,
    public userService: UserService
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
    if (!event || !event.route.url || !event.route.url._value || !event.route.url._value[0])
      return;
    this.titleService.setTitle(`${this.title} - ${event.title}`);
    if (event.route.url._value[0].path.includes('login') ||
      event.route.url._value[0].path.includes('register') ||
      event.route.url._value[0].path.includes('setup') ||
      event.title === 'PAGE NOT FOUND' ||
      event.route.url._value[0].path.includes('forbidden')
    ) { this.hideHeader = true; }
    else this.hideHeader = false;
  }

  ngOnInit(): void {
    const lang = localStorage.getItem("lang");
    if (this.getWidth() > 800) {
      this.opened = false;
    }
  }

  getWidth(): number { return window.innerWidth; }

}