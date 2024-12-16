import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SnackBarService } from '../../../core/services/snack-bar.service';
import { BudgetService } from '../../../core/services/budget.service';
import { Budget } from '../../../core/models/Budget';
import { CategoryService } from '../../../core/services/category.service';
import { Category } from '../../../core/models/Category';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

@Component({
    selector: 'app-add-update-budget',
    standalone:true,
    imports: [
        ReactiveFormsModule,
        RouterLink
    ],
    templateUrl: './add-update-budget.component.html',
    styleUrl: './add-update-budget.component.scss'
})
export class AddUpdateBudgetComponent implements OnInit {
    budgetForm!: FormGroup;
    categories: String[]=[];
    snackBarService= inject(SnackBarService)
    budgetService= inject(BudgetService)
    categoryService = inject(CategoryService)
    budgetId!: number
    amount!: number
    startDate!: Date
    endDate!: Date
    categoryName!: string

    constructor(private formBuilder: FormBuilder,private activatedRoute: ActivatedRoute, private route:Router) 
    { }
  
    ngOnInit(){
      this.budgetForm = this.formBuilder.group({
          amount: ['', [Validators.required, Validators.min(0)]],
          startDate: ['', Validators.required],
          endDate: ['', Validators.required],
          category: ['', Validators.required],
      });

        this.activatedRoute.queryParams.subscribe(params => {
          this.budgetId = params['budgetId'];
          this.amount = params['amount'];
          this.startDate = params['startDate'];
          this.endDate = params['endDate'];
          this.categoryName = params['categoryName'];

      
          if (this.budgetId) {
            this.budgetForm.patchValue({
              amount: this.amount || '',
              startDate: this.startDate || '',
              endDate: this.endDate || '',
              category: this.categoryName || '',
            });
          }
        });
        this.getAllCategories();
    }

    getAllCategories() {
      this.categoryService.getAll().subscribe({
        next: (data: Category[]) => {
          this.categories = data.map(category => category.name);
        },
        error: (error) => {
          console.error('Error loading categories:', error);
          this.snackBarService.error('Failed to load categories');
        }
      });
    }

    onSubmit() {
      if (this.budgetForm.valid) {
        const { amount, startDate, endDate, category } = this.budgetForm.value;
    
        const budgetModel: Budget = {
          Amount: amount,
          StartDate: startDate,
          EndDate: endDate,
          CategoryName: category,
        };
        if(this.budgetId != null)
        {
          this.budgetService.update(budgetModel, this.budgetId).subscribe({
            next: () => {
              this.snackBarService.success('Budget Updated successfully');
              this.route.navigate(['/budgets']).then(() => {
                window.location.reload();
              });
            },
            error: (error) => {
              console.error('Error:', error);
              this.snackBarService.error('Budget Update failed');
            },
          });
        }
        else{
          this.budgetService.create(budgetModel).subscribe({
            next: () => {
              this.snackBarService.success('Budget created successfully');
              this.route.navigate(['/budgets']).then(() => {
                window.location.reload();
              });
            },
            error: (error) => {
              console.error('Error:', error);
              this.snackBarService.error('Budget creation failed');
            },
          });
        }

      } else {
        this.snackBarService.error('Please fill all required fields');
      }
    }
    
  
    onCancel() {
      this.budgetForm.reset();
    }
}
