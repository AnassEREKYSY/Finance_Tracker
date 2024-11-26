import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SnackBarService } from '../../../core/services/snack-bar.service';
import { BudgetService } from '../../../core/services/budget.service';
import { Budget } from '../../../core/models/Budget';

@Component({
    selector: 'app-add-update-budget',
    standalone:true,
    imports: [
        ReactiveFormsModule
    ],
    templateUrl: './add-update-budget.component.html',
    styleUrl: './add-update-budget.component.scss'
})
export class AddUpdateBudgetComponent implements OnInit {
    budgetForm!: FormGroup;
    snackBarService= inject(SnackBarService)
    budgetService= inject(BudgetService)

    constructor(private formBuilder: FormBuilder) 
    { }
  
    ngOnInit(){
        this.budgetForm = this.formBuilder.group({
            amount: ['', [Validators.required, Validators.min(0)]],
            startDate: ['', Validators.required],
            endDate: ['', Validators.required],
            category: ['', Validators.required],
        });
    }

    onSubmit() {
        if (this.budgetForm.valid) {
            const budgetModel: Budget = 
            {
              Amount:this.budgetForm.value.amount,
              StartDate:this.budgetForm.value.startDate,
              EndDate:this.budgetForm.value.endDate,
              CategoryName:this.budgetForm.value.category
            };
      
            this.budgetService.create(budgetModel).subscribe({
              next: () => {
                this.snackBarService.success('Budget created successful');
              },
              error: () => {
                this.snackBarService.error('Budget creation failed');
              },
            });
        }else {
            this.snackBarService.error('Please fill all required fields');
        }
    }
  
    onCancel() {
      this.budgetForm.reset();
    }
}
