import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { NavBarComponent } from "../../shared/nav-bar/nav-bar.component";

@Component({
  selector: 'app-start',
  standalone: true,
  imports: [NavBarComponent],
  templateUrl: './start.component.html',
  styleUrl: './start.component.scss'
})
export class StartComponent {

}
