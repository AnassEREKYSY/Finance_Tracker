import { Component } from '@angular/core';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatButtonModule } from '@angular/material/button'; // For mat-button
import { MatToolbarModule } from '@angular/material/toolbar'; // For Material toolbar, if needed
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
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
