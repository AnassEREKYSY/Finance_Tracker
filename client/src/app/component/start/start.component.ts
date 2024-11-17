import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { NavBarComponent } from "../../shared/nav-bar/nav-bar.component";
import { TransactionTableComponent } from "../transaction-table/transaction-table.component";

@Component({
  selector: 'app-start',
  standalone: true,
  imports: [NavBarComponent, TransactionTableComponent],
  templateUrl: './start.component.html',
  styleUrl: './start.component.scss'
})
export class StartComponent {

}
