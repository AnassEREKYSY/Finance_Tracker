import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Transaction } from '../../../core/models/Transaction';
import { ChartOptions, ChartData, registerables } from 'chart.js';
import { NgChartsModule } from 'ng2-charts';
import { TransactionService } from '../../../core/services/transaction.service';
import { Chart } from 'chart.js';
import { Budget } from '../../../core/models/Budget';

Chart.register(...registerables);

@Component({
  selector: 'app-transaction-chart',
  templateUrl: './transaction-chart.component.html',
  styleUrls: ['./transaction-chart.component.scss'],
  standalone: true,
  imports: [NgChartsModule],
})
export class TransactionChartComponent implements OnInit, OnChanges {
  @Input() startDate!: Date;
  @Input() endDate!: Date;
  @Input() categoryName!: string;
  @Input() budget!: Budget;

  transactions: Transaction[] = [];
  expenseSum: number = 0;

  public pieChartLabels: string[] = ['Rest', 'Expense'];
  public pieChartData: ChartData<'pie'> = {
    labels: this.pieChartLabels,
    datasets: [
      {
        data: [],
        backgroundColor: ['#FF6384', '#36A2EB'],
      },
    ],
  };
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
        next: (response: Transaction[]) => {
          this.transactions = response || [];
          this.aggregateTransactionData();
        },
        error: (err: any) => {
          console.error('Error fetching transactions:', err);
          this.transactions = [];
          this.aggregateTransactionData();
        },
      });
  }
  
  

  aggregateTransactionData() {
    if (!this.transactions || this.transactions.length === 0) {
      this.pieChartData.datasets[0].data = [100, 0];
      this.pieChartLabels = ['Rest', 'Expense'];
      this.pieChartData.datasets[0].backgroundColor = ['#4CAF50', '#BDBDBD'];
      return;
    }
  
    const expenseSum = this.transactions
      .filter((t) => t.type && t.type.toLowerCase() === 'expense')
      .reduce((sum, t) => sum + t.amount, 0);
  
  
    if (expenseSum === 0) {
      this.pieChartData.datasets[0].data = [100, 0];
      this.pieChartLabels = ['Rest', 'Expense'];
      this.pieChartData.datasets[0].backgroundColor = ['#4CAF50', '#BDBDBD'];
    } else {
      const remaining = this.budget.Amount - expenseSum;
  
      this.pieChartData.datasets[0].data = [expenseSum, Math.max(remaining, 0)];
      this.pieChartLabels = ['Expense', 'Rest'];
      this.pieChartData.datasets[0].backgroundColor = ['#BDBDBD', '#4CAF50'];
    }
  }
  
  
  
  
  
  
}