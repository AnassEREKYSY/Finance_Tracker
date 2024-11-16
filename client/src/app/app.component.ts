import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { StartComponent } from "./component/start/start.component";
import { NavBarComponent } from "./shared/nav-bar/nav-bar.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, StartComponent, NavBarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'client';
}
