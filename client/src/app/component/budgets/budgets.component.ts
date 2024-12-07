import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe, CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { Budget } from '../../core/models/Budget';
import { BudgetService } from '../../core/services/budget.service';

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
  budgetsData!: Array<Budget>;
  budgetServcie= inject(BudgetService)
  budgets: Budget[] = [];

  constructor(private route:Router){}

  ngOnInit(): void {
    this.getAllBudgets();
  }

  getAllBudgets() {
    this.budgetServcie.getAll().subscribe({
      next: (data: Array<any>) => {
  
        this.budgetsData = data;
  
        this.budgets = data.map(budget => ({
          BudgetId: budget.budgetId,
          Amount: budget.amount,
          StartDate: new Date(budget.startDate),
          EndDate: new Date(budget.endDate),
          CategoryName: budget.categoryName,
        }));
  
      },
      error: (err) => {
        console.error('Error fetching budgets:', err);
      },
    });
  }
  

  editBudget(budget: Budget): void {
    this.route.navigate(['budgets/addUpdate'], {
      queryParams: { 
        budgetId: budget.BudgetId, 
        amount: budget.Amount,
        startDate: new Date(budget.StartDate).toISOString().split('T')[0],
        endDate: new Date(budget.EndDate).toISOString().split('T')[0],
        categoryName: budget.CategoryName
      }
    });
  }
  

  deleteBudget(budget: Budget): void {
    if (!budget.BudgetId) {
      console.error('Cannot delete a budget without an Id');
      return;
    }
  
    this.budgetServcie.delete(budget.BudgetId).subscribe({
      next: () => {
        this.getAllBudgets();
      },
      error: (err) => {
        console.error('Error deleting budget:', err);
      },
    });
  }
}