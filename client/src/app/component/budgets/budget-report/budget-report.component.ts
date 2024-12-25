import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ReportService } from '../../../core/services/report.service';
import { SnackBarService } from '../../../core/services/snack-bar.service';
import { Router, RouterLink } from '@angular/router';
import { BudgetService } from '../../../core/services/budget.service';
import { Budget } from '../../../core/models/Budget';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-budget-report',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    NgSelectModule
  ],
  templateUrl: './budget-report.component.html',
  styleUrl: './budget-report.component.scss'
})
export class BudgetReportComponent implements OnInit{
  budgetReportForm!: FormGroup;
  snackBarService= inject(SnackBarService)
  reportService= inject(ReportService)
  budgetService= inject(BudgetService)
  budgets:Budget[]=[];

  constructor(private formBuilder: FormBuilder, private route:Router) 
  { }


  ngOnInit(): void {
    this.budgetReportForm = this.formBuilder.group({
      budget: [[], Validators.required]
    });
    this.loadAllBudgets();
    console.log(this.budgets)
  }

  loadAllBudgets() {
    this.budgetService.getAll().subscribe({
      next: (data: Array<any>) => {
        console.log(data)
        this.budgets = data.map(budget => ({
          BudgetId: budget.budgetId,
          Amount: budget.amount,
          StartDate: budget.startDate, 
          EndDate: budget.endDate,
          CategoryName: budget.categoryName,
        }));
      },
      error: (err) => {
        console.error('Error fetching budgets:', err);
      },
    });
  }
  

  onSubmit() {
    if (this.budgetReportForm.valid) {
      const selectedBudgetIds = this.budgetReportForm.get('budget')?.value;

      if (selectedBudgetIds && Array.isArray(selectedBudgetIds)) {
        this.reportService.generateBudgetReport(selectedBudgetIds).subscribe({
          next: (response) => {
            const blob = new Blob([response], { type: 'application/pdf' });
            const link = document.createElement('a');
            link.href = URL.createObjectURL(blob);
            link.download = 'budget_report.pdf'; 
            link.click();
          },
          error: (err) => {
            console.error('Error generating budget report:', err);
            this.snackBarService.error('Error generating the report.');
          }
        });
      } else {
        this.snackBarService.error('Please select at least one budget.');
      }
    } else {
      this.snackBarService.error('Please fill out the form correctly.');
    }
  }
}
