import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { SnackBarService } from '../../../core/services/snack-bar.service';
import { CategoryService } from '../../../core/services/category.service';
import { Category } from '../../../core/models/Category';

@Component({
  selector: 'app-add-update-category',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './add-update-category.component.html',
  styleUrls: ['./add-update-category.component.scss']
})
export class AddUpdateCategoryComponent implements OnInit {
  categoryForm!: FormGroup;
  snackBarService=  inject(SnackBarService)
  categoryService= inject(CategoryService)

  constructor(private formBuilder: FormBuilder) 
  { }

    ngOnInit(): void {
        this.categoryForm = this.formBuilder.group({
            categoryName: ['', Validators.required]
        });
    }

  onSubmit() {
    if (this.categoryForm.valid) {
        const categoryModel: Category = 
        {
          Name:this.categoryForm.value.categoryName,
        };
  
        this.categoryService.create(categoryModel).subscribe({
          next: () => {
            this.snackBarService.success('Category created successful');
          },
          error: () => {
            this.snackBarService.error('Category creation failed');
          },
        });
    }else {
        this.snackBarService.error('Please fill all required fields');
    }
  }

  onCancel() {
    this.categoryForm.reset();
  }
}
