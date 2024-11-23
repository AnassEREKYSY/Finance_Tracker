import { Component, Input, OnInit, AfterViewInit, OnDestroy, OnChanges, SimpleChanges, ChangeDetectionStrategy, ViewChild, ElementRef } from '@angular/core';
import { Chart, registerables, ChartConfiguration, ChartOptions, ChartType } from 'chart.js';

@Component({
  selector: 'app-budget',
  standalone: true,
  templateUrl: './budget.component.html',
  styleUrls: ['./budget.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BudgetComponent implements OnInit, AfterViewInit, OnDestroy, OnChanges {
  @Input() chartData: number[] = [];
  @Input() chartLabels: string[] = [];
  @Input() chartOptions: ChartOptions<'pie'> = {
    responsive: true,
    maintainAspectRatio: false, // Allows custom sizing
    plugins: {
      legend: {
        position: 'top', // 'top', 'left', 'right', 'bottom'
      },
      tooltip: {
        enabled: true,
      },
    },
  };

  @ViewChild('chartCanvas', { static: false }) chartCanvas!: ElementRef;

  private chartInstance: Chart<'pie'> | null = null; // Ensure type is specific to 'pie'

  ngOnInit() {
    Chart.register(...registerables);
    Chart.defaults.plugins.legend.position = 'top';
    Chart.defaults.maintainAspectRatio = false;
  }

  ngAfterViewInit() {
    this.createChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['chartData']?.currentValue || changes['chartLabels']?.currentValue || changes['chartOptions']?.currentValue) {
      this.updateChart();
    }
  }

  ngOnDestroy() {
    this.destroyChart();
  }

  private createChart() {
    this.destroyChart();
    if (this.chartCanvas) {
      const config: ChartConfiguration<'pie'> = {
        type: 'pie',
        data: {
          labels: this.chartLabels,
          datasets: [
            {
              data: this.chartData,
              backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF'],
            },
          ],
        },
        options: this.chartOptions, // This must be of type `ChartOptions<'pie'>`
      };
      this.chartInstance = new Chart(this.chartCanvas.nativeElement, config);
    }
  }

  private updateChart() {
    if (this.chartInstance) {
      this.chartInstance.data.labels = this.chartLabels;
      this.chartInstance.data.datasets[0].data = this.chartData;
      this.chartInstance.update();
    } else {
      this.createChart();
    }
  }

  private destroyChart() {
    if (this.chartInstance) {
      this.chartInstance.destroy();
      this.chartInstance = null;
    }
  }
}
