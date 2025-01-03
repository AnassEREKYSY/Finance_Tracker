import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { SnackBarService } from '../../../core/services/snack-bar.service';
import { CategoryService } from '../../../core/services/category.service';
import { Category } from '../../../core/models/Category';
import { ActivatedRoute, Route, Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-add-update-category',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './add-update-category.component.html',
  styleUrls: ['./add-update-category.component.scss']
})
export class AddUpdateCategoryComponent implements OnInit {
  categoryForm!: FormGroup;
  snackBarService=  inject(SnackBarService)
  categoryService= inject(CategoryService)
  name: string | null = null;
  categoryId!: number;

  constructor(private formBuilder: FormBuilder,private activatedRoute: ActivatedRoute, private route:Router) 
  { }

  ngOnInit(): void {
    this.categoryForm = this.formBuilder.group({
      categoryName: ['', Validators.required]
    });
  
    this.activatedRoute.queryParams.subscribe(params => {
      this.categoryId = params['categoryId'];
      this.name = params['name'];
  
      if (this.name) {
        this.categoryForm.patchValue({
          categoryName: this.name,
        });
      }
    });
  }
  

  onSubmit() {
    if (this.categoryForm.valid) {
        const categoryModel: Category = 
        {
          name:this.categoryForm.value.categoryName,
        };
        if(this.categoryId != null)
        {
          this.categoryService.update(this.categoryId,categoryModel).subscribe({
            next: () => {
              this.snackBarService.success('Category updated successful');
              this.route.navigate(['/categories']).then(() => {
                window.location.reload();
              });
            },
            error: () => {
              this.snackBarService.error('Category update failed');
            },
          });
        }
        else{
          this.categoryService.create(categoryModel).subscribe({
            next: () => {
              this.snackBarService.success('Category created successful');
              this.route.navigate(['/categories']).then(() => {
                window.location.reload();
              });
            },
            error: () => {
              this.snackBarService.error('Category creation failed');
            },
          });
        }

    }else {
        this.snackBarService.error('Please fill all required fields');
    }
  }

  onCancel() {
    this.categoryForm.reset();
  }
}
