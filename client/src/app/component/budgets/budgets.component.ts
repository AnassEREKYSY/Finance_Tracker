import { Component, inject, OnInit } from '@angular/core';
import { CurrencyPipe, DatePipe, CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { Budget } from '../../core/models/Budget';
import { BudgetService } from '../../core/services/budget.service';
import { Transaction } from '../../core/models/Transaction';
import { TransactionChartComponent } from '../transactions/transaction-chart/transaction-chart.component';
import { TransactionService } from '../../core/services/transaction.service';

@Component({
    selector: 'app-budgets-list',
    standalone:true,
    imports: [
        CurrencyPipe,
        DatePipe,
        CommonModule,
        RouterLink,
        TransactionChartComponent
    ],
    templateUrl: './budgets.component.html',
    styleUrl: './budgets.component.scss'
})
export class BudgetsComponent implements OnInit {
  budgetsData!: Array<Budget>;
  budgetServcie= inject(BudgetService)
  transactionServcie= inject(TransactionService)
  budgets: Budget[] = [];

  constructor(private route:Router){}

  ngOnInit(): void {
    this.getAllBudgets();
  }

  getTransactionsForCategory(categoryName: string): Promise<number> {
    return new Promise((resolve, reject) => {
      this.transactionServcie.getTransactionForCategory(categoryName).subscribe({
        next: (transactions: Array<Transaction>) => {
          const totalExpenses = transactions
            .filter(transaction => transaction.Type === 'Expense')
            .reduce((sum, transaction) => sum + transaction.Amount, 0);
          resolve(totalExpenses);
        },
        error: (err) => {
          console.error('Error fetching transactions for:', categoryName, err);
          reject(err);
        },
      });
    });
  }
  

  getAllBudgets() {
    this.budgetServcie.getAll().subscribe({
      next: async (data: Array<any>) => {
        const budgetPromises = data.map(async (budget) => {
          const totalExpenses = await this.getTransactionsForCategory(budget.categoryName);
          const rest = budget.amount - totalExpenses;
  
          return {
            BudgetId: budget.budgetId,
            Amount: budget.amount,
            StartDate: new Date(budget.startDate),
            EndDate: new Date(budget.endDate),
            CategoryName: budget.categoryName,
            TotalExpenses: totalExpenses,
            Rest: rest
          };
        });
  
        this.budgets = await Promise.all(budgetPromises);
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