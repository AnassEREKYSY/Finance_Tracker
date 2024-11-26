import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TransactionService } from '../../../core/services/transaction.service';
import { SnackBarService } from '../../../core/services/snack-bar.service';
import { Transaction } from '../../../core/models/Transaction';

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
    transactionService= inject(TransactionService);
    snackBarService= inject(SnackBarService)

    constructor(private fb: FormBuilder) {
    }
    ngOnInit(): void {
        this.transactionForm = this.fb.group({
            transactionType: ['income', Validators.required],
            amount: [null, [Validators.required, Validators.min(1)]],
            description: [''],
            date: [null, Validators.required],
            category: ['', Validators.required],
          });
    }
  
    onSubmit() {
      if (this.transactionForm.valid) {
        const transactionModel: Transaction = 
        {
          Amount:this.transactionForm.value.amount,
          Description:this.transactionForm.value.description,
          Date:this.transactionForm.value.date,
          Type:this.transactionForm.value.type,
          CategoryName:this.transactionForm.value.category
        };
  
        this.transactionService.create(transactionModel).subscribe({
          next: () => {
            this.snackBarService.success('Transaction created successful');
          },
          error: () => {
            this.snackBarService.error('Transaction creation failed');
          },
        });
      }else {
          this.snackBarService.error('Please fill all required fields');
      }
    }
  
    onCancel() {
      this.transactionForm.reset();
    }
}
