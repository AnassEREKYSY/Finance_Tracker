import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CategoryService } from '../../core/services/category.service';
import { Category } from '../../core/models/Category';
import { SnackBarService } from '../../core/services/snack-bar.service';

@Component({
    selector: 'app-categories',
    standalone:true,
    imports: [
        CommonModule,
        RouterLink
    ],
    templateUrl: './categories.component.html',
    styleUrl: './categories.component.scss'
})
export class CategoriesComponent implements OnInit {
  categories: Category[] = [];
  categoryService =  inject(CategoryService)
  snackBarService = inject(SnackBarService)

  constructor(private route:Router){}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe({
      next: (data: Category[]) => {
        this.categories = data.map(category => ({
          categoryId: category.categoryId,
          name: category.name,
        }));
      },
      error: (error) => {
        console.error('Error loading categories:', error);
        this.snackBarService.error('Failed to load categories');
      }
    });
  }

  editCategory(item: Category): void {
    this.route.navigate(['categories/addUpdate'], {
      queryParams: { 
        categoryId: item.categoryId, 
        name: item.name 
      }
    });
  }

  deleteCategory(item: Category): void {
    this.categoryService.delete(item.categoryId!).subscribe({
      next: (response) => {
        if (response) {
          this.snackBarService.success('Category deleted successfully');
          this.loadCategories();
        } else {
          this.snackBarService.error('Failed to delete the category');
        }
      },
      error: (error) => {
        console.error('Error deleting category:', error);
        this.snackBarService.error('Failed to delete the category');
      }
    });
  }
  
}