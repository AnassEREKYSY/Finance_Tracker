import { Component } from '@angular/core';

@Component({
    selector: 'app-home',
    standalone:true,
    imports: [],
    templateUrl: './home.component.html',
    styleUrl: './home.component.scss'
})
export class HomeComponent {
  dropdown: string | null = null;

  toggleDropdown(menu: string) {
    this.dropdown = this.dropdown === menu ? null : menu;
  }
}
