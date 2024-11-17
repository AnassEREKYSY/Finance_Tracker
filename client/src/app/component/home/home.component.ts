import { Component } from '@angular/core';
import { TransactionTableComponent } from "../transaction-table/transaction-table.component";
import { NavBarComponent } from "../../shared/nav-bar/nav-bar.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [TransactionTableComponent, NavBarComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}
