import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [
    CurrencyPipe,
    CommonModule,
    RouterLink
  ],
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.scss'
})
export class CategoriesComponent implements OnInit {
  categories: { name: string; transactionsCount: number }[] = [];

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.categories = [
      { name: 'Rent', transactionsCount: 5 },
      { name: 'Groceries', transactionsCount: 8 },
      { name: 'Utilities', transactionsCount: 2 },
      { name: 'Entertainment', transactionsCount: 3 },
      { name: 'Transportation', transactionsCount: 6 },
    ];
  }

  editCategory(item: any): void {
    console.log('Editing category', item);
  }

  deleteCategory(item: any): void {
    console.log('Deleting category', item);
  }
}