import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TransactionService } from '../../../core/services/transaction.service';
import { SnackBarService } from '../../../core/services/snack-bar.service';
import { Transaction } from '../../../core/models/Transaction';
import { CategoryService } from '../../../core/services/category.service';
import { Category } from '../../../core/models/Category';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'app-add-update-transaction',
    imports: [
        ReactiveFormsModule
    ],
    templateUrl: './add-update-transaction.component.html',
    styleUrl: './add-update-transaction.component.scss'
})
export class AddUpdateTransactionComponent implements OnInit{
    transactionForm!: FormGroup;
    categories: String[]=[];
    transactionService= inject(TransactionService);
    snackBarService= inject(SnackBarService)
    categoryService = inject(CategoryService)

    transactionId!: number;
    amount!:number;
    description!:string;
    date!:Date;
    categoryName!:string;


    constructor(private fb: FormBuilder, private route:Router, private activatedRoute: ActivatedRoute) {
    }
    ngOnInit(): void {
        this.transactionForm = this.fb.group({
            transactionType: ['income', Validators.required],
            amount: [null, [Validators.required, Validators.min(1)]],
            description: [''],
            date: [null, Validators.required],
            category: ['', Validators.required],
          });

          this.activatedRoute.queryParams.subscribe(params => {
            this.transactionId = params['transactionId'];
            this.amount = params['amount'];
            this.date = params['date'];
            this.description = params['description'];
            this.categoryName = params['categoryName'];
  
        
            if (this.transactionId) {
              this.transactionForm.patchValue({
                amount: this.amount || '',
                date: this.date || '',
                description: this.description || '',
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
      if (this.transactionForm.valid) {
        const transactionModel: Transaction = 
        {
          Amount:this.transactionForm.value.amount,
          Description:this.transactionForm.value.description,
          TransactionDate:this.transactionForm.value.date,
          Type:this.transactionForm.value.transactionType,
          CategoryName:this.transactionForm.value.category
        };
        if(this.transactionId!=null)
        {
          // this.transactionService.update(transactionModel, this.transactionId).subscribe({
          //   next: () => {
          //     this.snackBarService.success('Transaction updated successful');
          //     this.route.navigate(['/transactions']).then(() => {
          //       window.location.reload();
          //     });
          //   },
          //   error: () => {
          //     this.snackBarService.error('Transaction update failed');
          //   },
          // });
        }
        else{
          console.log(transactionModel)
          this.transactionService.create(transactionModel).subscribe({
            next: () => {
              this.snackBarService.success('Transaction created successful');
              this.route.navigate(['/transactions']).then(() => {
                window.location.reload();
              });
            },
            error: () => {
              this.snackBarService.error('Transaction creation failed');
            },
          });
        }
  

      }else {
          this.snackBarService.error('Please fill all required fields');
      }
    }
  
    onCancel() {
      this.transactionForm.reset();
    }
}
