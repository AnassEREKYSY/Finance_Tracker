import { Component } from '@angular/core';
import { NotificationsComponent } from '../notifications/notifications.component';

@Component({
    selector: 'app-home',
    standalone:true,
    imports: [
      NotificationsComponent
    ],
    templateUrl: './home.component.html',
    styleUrl: './home.component.scss'
})
export class HomeComponent {
  dropdown: string | null = null;
  isAuthenticated = sessionStorage.getItem('authToken') != null ?true:false;

  toggleDropdown(menu: string) {
    this.dropdown = this.dropdown === menu ? null : menu;
  }
}
