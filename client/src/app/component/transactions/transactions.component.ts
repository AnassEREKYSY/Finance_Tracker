import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Transaction } from '../../core/models/Transaction';
import { TransactionService } from '../../core/services/transaction.service';

@Component({
    selector: 'app-transactions',
    imports: [
        CurrencyPipe,
        DatePipe,
        CommonModule,
        RouterLink
    ],
    templateUrl: './transactions.component.html',
    styleUrl: './transactions.component.scss'
})
export class TransactionsComponent implements OnInit{
  transactions: Transaction[] = [];
  transactionService = inject(TransactionService);

  constructor(private route:Router){}

  ngOnInit(): void {
    this.loadTransactions();
  }

  loadTransactions(): void {
    this.transactionService.getAll().subscribe({
      next: (data: Array<any>) => {
        console.log(data)
        this.transactions = data.map(transaction => ({
          Id: transaction.transactionId?.toString(),
          Amount: Number(transaction.amount),
          Description: transaction.description || "No Description",
          TransactionDate: transaction.TransactionDate ? new Date(transaction.TransactionDate) : null,
          Type: transaction.type || "Unknown",
          CategoryName: transaction.categoryName || "Uncategorized",
        }));
      },
      error: (err) => {
        console.error('Error fetching transactions:', err);
      },
    });
  }
  

  editTransaction(item: Transaction): void {
    this.route.navigate(['transactions/addUpdate'], {
      queryParams: { 
        transactionId: item.Id, 
        amount: item.Amount,
        date: new Date(item.TransactionDate!).toISOString().split('T')[0],
        description: item.Description,
        categoryName: item.CategoryName
      }
    });
  }
  
  deleteTransaction(item: Transaction): void {
    if (!item.Id) {
      console.error('Cannot delete a Transaction without an Id');
      return;
    }
  
    this.transactionService.delete(item.Id!).subscribe({
      next: () => {
        window.location.reload();
        this.loadTransactions();
      },
      error: (err) => {
        console.error('Error deleting transaction:', err);
      },
    });
  }

}