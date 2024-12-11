import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Transaction } from '../../../core/models/Transaction';
import { ChartOptions, ChartData, registerables } from 'chart.js';
import { NgChartsModule } from 'ng2-charts';
import { TransactionService } from '../../../core/services/transaction.service';
import { Chart } from 'chart.js';

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

  transactions: Transaction[] = [];
  incomeSum: number = 0;
  expenseSum: number = 0;

  public pieChartLabels: string[] = ['Income', 'Expense'];
  public pieChartData: ChartData<'pie'> = {
    labels: this.pieChartLabels,
    datasets: [
      {
        data: [], // This will be populated with the pie chart data
        backgroundColor: ['#36A2EB', '#FF6384'], // Example colors
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
          this.transactions = response;
          this.aggregateTransactionData();
        },
        error: (err: any) => {
          if (err === 'No transactions found for the given category and date range') {
            this.pieChartData.datasets[0].data = [100];
            this.pieChartLabels = ['No Transactions'];
            this.pieChartData.datasets[0].backgroundColor = ['#4CAF50']; 
          } else {
            console.error('Error fetching transactions:', err);
          }
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
      this.pieChartData.datasets[0].data = [100];
      this.pieChartLabels = ['No Transactions'];
      this.pieChartData.datasets[0].backgroundColor = ['#4CAF50']; // Green color for no transactions
    } else {
      this.pieChartData.datasets[0].data = [this.incomeSum, this.expenseSum];
      this.pieChartData.datasets[0].backgroundColor = ['#4CAF50', '#FFCDD2']; // Green for income, Light red for expense
    }
  }  
  
}
