import { Component } from '@angular/core';
import { BudgetComponent } from "./budget/budget.component";

@Component({
  selector: 'app-budgets-list',
  standalone: true,
  imports: [BudgetComponent],
  templateUrl: './budgets.component.html',
  styleUrl: './budgets.component.scss'
})
export class BudgetsComponent {
  pieChartData1 = [300, 200, 400, 100, 150];
  pieChartLabels1 = ['Food', 'Transportation', 'Rent', 'Groceries', 'Travel'];
  pieChartOptions1 = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top' as 'top' | 'bottom' | 'left' | 'right' | 'center',
      },
    },
  };

  pieChartData2 = [500, 100, 250, 200, 180];
  pieChartLabels2 = ['Health', 'Entertainment', 'Savings', 'Education', 'Insurance'];
  pieChartOptions2 = {
    responsive: true,
    plugins: {
      legend: {
        position: 'right' as 'top' | 'bottom' | 'left' | 'right' | 'center',
      },
    },
  };

  pieChartData3 = [600, 300, 200, 150, 100];
  pieChartLabels3 = ['Rent', 'Utilities', 'Internet', 'Transport', 'Groceries'];
  pieChartOptions3 = {
    responsive: true,
    plugins: {
      legend: {
        position: 'bottom' as 'top' | 'bottom' | 'left' | 'right' | 'center',
      },
    },
  };
}


