import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-transactions',
  standalone: true,
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
  transactions: { type: string; amount: number; description: string; date: string }[] = [];

  ngOnInit(): void {
    this.loadTransactions();
  }

  loadTransactions(): void {
    this.transactions = [
      { 
        type: 'Income', 
        amount: 1200, 
        description: 'Salary payment for October, received from my employer. This is my regular monthly payment and covers all my basic living expenses for the month.', 
        date: '2024-10-01' 
      },
      { 
        type: 'Expense', 
        amount: 300, 
        description: 'Grocery shopping for the week. Includes food, household items, and some seasonal vegetables to prepare healthy meals for the family.', 
        date: '2024-10-02' 
      },
      { 
        type: 'Income', 
        amount: 150, 
        description: 'Freelance project payment received from a client for the website design work I completed last week. The project was well-received, and the client was satisfied with the outcome.', 
        date: '2024-10-03' 
      },
      { 
        type: 'Expense', 
        amount: 500, 
        description: 'Car maintenance and repair costs for my vehicle. Includes oil change, tire replacement, and a minor engine checkup to ensure everything is running smoothly for the upcoming season.', 
        date: '2024-10-05' 
      },
      { 
        type: 'Expense', 
        amount: 200, 
        description: 'Dining out with friends at a popular restaurant. We had a great time catching up, and the bill included appetizers, main courses, and drinks for everyone.', 
        date: '2024-10-07' 
      },
      { 
        type: 'Income', 
        amount: 800, 
        description: 'Stock dividends from investments I made in the stock market last year. This is a quarterly payment that I reinvest into additional stocks to grow my portfolio further.', 
        date: '2024-10-10' 
      },
      { 
        type: 'Expense', 
        amount: 100, 
        description: 'Netflix subscription renewal. This is my monthly subscription for streaming entertainment, including a wide range of movies, series, and documentaries.', 
        date: '2024-10-12' 
      },
      { 
        type: 'Income', 
        amount: 250, 
        description: 'Bonus received from work. This was an additional incentive from the company for exceeding quarterly performance goals. It will be added to my monthly savings account.', 
        date: '2024-10-15' 
      }
    ];
  }

  editTransaction(item: any): void {
    console.log('Editing transaction', item);
  }
  
  deleteTransaction(item: any): void {
    console.log('Deleting transaction', item);
  }

  addTransaction(): void {
    console.log('Add New Transaction');
  }
}