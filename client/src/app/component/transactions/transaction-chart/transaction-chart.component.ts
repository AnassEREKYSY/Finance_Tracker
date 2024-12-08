import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Transaction } from '../../../core/models/Transaction';
import { ChartOptions, ChartData } from 'chart.js';
import { TransactionService } from '../../../core/services/transaction.service';

@Component({
  selector: 'app-transaction-chart',
  templateUrl: './transaction-chart.component.html',
  styleUrls: ['./transaction-chart.component.scss']
})
export class TransactionChartComponent implements OnInit, OnChanges {
  @Input() startDate!: Date;
  @Input() endDate!: Date;
  @Input() categoryName!: string;

  transactions: Transaction[] = [];
  incomeSum: number = 0;
  expenseSum: number = 0;

  public pieChartLabels: string[] = ['Income', 'Expense'];
  public pieChartData: number[] = [];
  public pieChartOptions: ChartOptions = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
      },
    },
  };

  constructor(private transactionService: TransactionService) {}

  ngOnInit(): void {
    this.getTransactionForCategory();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['startDate'] || changes['endDate'] || changes['categoryName']) {
      this.getTransactionForCategory();
    }
  }

  getTransactionForCategory() {
    this.transactionService
      .getTransactionsForCategory(this.startDate, this.endDate, this.categoryName)
      .subscribe({
        next: (response) => {
          this.transactions = response;
          this.aggregateTransactionData();
        },
        error: (err) => {
          console.error('Error fetching transactions:', err);
        },
      });
  }

  aggregateTransactionData() {
    this.incomeSum = this.transactions
      .filter((t) => t.Type === 'Income')
      .reduce((sum, t) => sum + t.Amount, 0);

    this.expenseSum = this.transactions
      .filter((t) => t.Type === 'Expense')
      .reduce((sum, t) => sum + t.Amount, 0);

    if (this.incomeSum === 0 && this.expenseSum === 0) {
      // Set default fallback to green
      this.pieChartData = [100]; // 100% green segment when no transactions
      this.pieChartLabels = ['No Transactions'];
    } else {
      this.pieChartData = [this.incomeSum, this.expenseSum];
    }
  }
}
