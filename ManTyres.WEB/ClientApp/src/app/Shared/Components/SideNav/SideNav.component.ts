import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/Account/Services/user.service';
import { UserRole } from '../../Models/enums';

@Component({
  selector: 'app-sidenav',
  templateUrl: './SideNav.component.html',
  styleUrls: ['./SideNav.component.scss']
})
export class SideNavComponent implements OnInit {
  activeNav: Icon | null = null;
  patchOff: boolean = false;
  icons: Icon[] = [];

  userRole: UserRole = UserRole.Administrator;

  constructor(
    private userService: UserService,
  ) { }

  navBarInteraction(navElement: Icon): void {
    this.activeNav = navElement;
  }

  ngOnInit(): void {
    var test = this.userService.userRole;
    //this.userRole = this.userService.userRole;
    this.initNavBarManager();
  }

  getWidth(): number {
    return window.innerWidth;
  }

  initNavBarManager(): void {
    switch (this.userRole) {
      case UserRole.Developer:
      case UserRole.Administrator: {
        this.icons = [
          { label: "content/Claims-Dashboard", icon: "fas fa-clipboard-check" },
          { label: "content/EndUsers", icon: "fas fa-users" },
          { label: "content/Technicians", icon: "fas fa-toolbox" },
          { label: "content/Equipments", icon: "fas fa-tools" },
          { label: "Map", icon: "fas fa-globe" },
          { label: "content/Distributors", icon: "fas fa-building" },
          { label: "content/Workshops", icon: "icon-WorkshopIcon_TeqLink" },
          { label: "content/DistributorQuotations", icon: "fas fa-credit-card" },
          { label: "content/Requests", icon: "fas fa-envelope-open-text" },
        ];
        break;
      }
      case UserRole.Customer: {
        this.icons = [
          { label: "content/Claims-Dashboard", icon: "fas fa-clipboard-check" }
        ];
        break;
      }
      case UserRole.Worker: {
        this.icons = [
          { label: "content/Claims-Dashboard", icon: "fas fa-clipboard-check" },
          { label: "content/Distributors", icon: "fas fa-building" },
          { label: "content/Workshops", icon: "icon-WorkshopIcon_TeqLink" },
          { label: "Map", icon: "fas fa-globe" },
          { label: "content/DistributorQuotations", icon: "fas fa-credit-card" },
          { label: "content/Equipments", icon: "fas fa-tools" },
          { label: "content/Technicians", icon: "fas fa-toolbox" },
        ];
        break;
      }
      case UserRole.Marketer: {
        this.icons = [
          { label: "content/Subscriptions", icon: "fas fa-check-circle" },
          { label: "content/Requests", icon: "fas fa-envelope-open-text" },
          { label: "content/Distributors", icon: "fas fa-building" },
          { label: "content/Workshops", icon: "icon-WorkshopIcon_TeqLink" },
          { label: "Map", icon: "fas fa-globe" },
          { label: "content/Equipments", icon: "fas fa-tools" },
          { label: "content/Tests", icon: "fas fa-car" },
          { label: "content/Claims-Dashboard", icon: "fas fa-clipboard-check" },
        ];
        break;
      }
      case UserRole.Manager: {
        this.icons = [
          { label: "content/Subscriptions", icon: "fas fa-check-circle" },
          { label: "content/Claims-Dashboard", icon: "fas fa-clipboard-check" },
          { label: "content/EndUsers", icon: "fas fa-users" },
          { label: "content/Workshops", icon: "icon-WorkshopIcon_TeqLink" },
          { label: "content/Distributors", icon: "fas fa-building" },
          { label: "Map", icon: "fas fa-globe" },
          { label: "content/DistributorQuotations", icon: "fas fa-credit-card" },
          { label: "content/Requests", icon: "fas fa-envelope-open-text" },
          { label: "content/Equipments", icon: "fas fa-tools" },
          { label: "content/Technicians", icon: "fas fa-toolbox" },
          { label: "content/Tests", icon: "fas fa-car" },
        ];
        break;
      }
    }
  }
}
export interface Icon {
  label: string;
  icon: string;
}