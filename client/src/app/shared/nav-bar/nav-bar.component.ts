import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-nav-bar',
    standalone:true,
    imports: [
        RouterLink,
        CommonModule
    ],
    templateUrl: './nav-bar.component.html',
    styleUrl: './nav-bar.component.scss'
})
export class NavBarComponent {
  dropdown: string | null = null;

  toggleDropdown(menu: string) {
    this.dropdown = this.dropdown === menu ? null : menu;
  }
}
