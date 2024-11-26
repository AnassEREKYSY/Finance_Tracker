import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe, CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
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
  budgets: { Amount: number; StartDate: Date; EndDate: Date; CategoryName: string }[] = [];

  ngOnInit(): void {
    this.getAllBudgets();
  }

  getAllBudgets(){
    this.budgetServcie.getAll().subscribe({
      next: (data: Array<Budget>) => {
        this.budgetsData = data;

        this.budgets = data.map(budget => ({
          Amount: budget.Amount,
          StartDate: budget.StartDate,
          EndDate: budget.EndDate,
          CategoryName: budget.CategoryName,
        }));
      },
      error: (err) => {
        console.error('Error fetching budgets:', err);
      },
    });
  }

  editBudget(budget: Budget): void {
    if (!budget.Id) {
      console.error('Cannot update a budget without an Id');
      return;
    }
  
    this.budgetServcie.update(budget, budget.Id).subscribe({
      next: () => {
        this.getAllBudgets();
      },
      error: (err) => {
        console.error('Error updating budget:', err);
      },
    });
  }
  

  deleteBudget(budget: any): void {
    if (!budget.Id) {
      console.error('Cannot delete a budget without an Id');
      return;
    }
  
    this.budgetServcie.delete(budget.Id).subscribe({
      next: () => {
        this.getAllBudgets();
      },
      error: (err) => {
        console.error('Error deleting budget:', err);
      },
    });
  }
}