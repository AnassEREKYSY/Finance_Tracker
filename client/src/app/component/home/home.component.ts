import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from "../../shared/nav-bar/nav-bar.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NavBarComponent, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  dropdown: string | null = null;

  toggleDropdown(menu: string) {
    this.dropdown = this.dropdown === menu ? null : menu;
  }
}
