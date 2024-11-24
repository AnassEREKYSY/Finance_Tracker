import { Component, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe, CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-budgets-list',
    standalone:true,
    imports: [
        CurrencyPipe,
        DatePipe,
        CommonModule,
        RouterLink
    ],
    templateUrl: './budgets.component.html',
    styleUrl: './budgets.component.scss'
})
export class BudgetsComponent implements OnInit {
  budgets: { category: string; amount: number; startDate: string; endDate: string }[] = [];

  ngOnInit(): void {
    this.loadBudgets();
  }

  loadBudgets(): void {
    this.budgets = [
      { 
        category: 'Groceries', 
        amount: 500, 
        startDate: '2024-10-01', 
        endDate: '2024-10-31' 
      },
      { 
        category: 'Rent', 
        amount: 1000, 
        startDate: '2024-10-01', 
        endDate: '2024-10-31' 
      },
      { 
        category: 'Entertainment', 
        amount: 150, 
        startDate: '2024-10-01', 
        endDate: '2024-10-31' 
      },
      { 
        category: 'Utilities', 
        amount: 200, 
        startDate: '2024-10-01', 
        endDate: '2024-10-15' 
      },
      { 
        category: 'Transport', 
        amount: 100, 
        startDate: '2024-10-01', 
        endDate: '2024-10-31' 
      }
    ];
  }

  editBudget(budget: any): void {
    console.log('Editing budget', budget);
  }

  deleteBudget(budget: any): void {
    console.log('Deleting budget', budget);
  }

  addBudget(): void {
    console.log('Add New Budget');
  }
}