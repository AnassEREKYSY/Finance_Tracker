import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { LoginService } from '../../core/services/login.service';
import { SnackBarService } from '../../core/services/snack-bar.service';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'app-nav-bar',
    standalone:true,
    imports: [
        RouterLink,
        CommonModule,
        MatIcon,
    ],
    templateUrl: './nav-bar.component.html',
    styleUrl: './nav-bar.component.scss'
})
export class NavBarComponent implements OnInit{
  dropdown: string | null = null;
  userConnected!: boolean
  loginService = inject(LoginService);
  snackBarService = inject(SnackBarService);

  constructor(private route: Router){}

  ngOnInit(): void {
    const token = localStorage.getItem('authToken');
    if (token) {
      this.userConnected = true;
    } else {
      this.userConnected = false;
    }
  }

  toggleDropdown(menu: string) {
    this.dropdown = this.dropdown === menu ? null : menu;
  }

  logout(){
    this.loginService.logOut().subscribe({
      next: () => {
        localStorage.removeItem('authToken');
        this.snackBarService.success('Logout successful');
        this.route.navigate(['/home']).then(() => {
          window.location.reload();
        });
      },
      error: () => {
        this.snackBarService.error('Logout failed');
      },
    });
  }
}
