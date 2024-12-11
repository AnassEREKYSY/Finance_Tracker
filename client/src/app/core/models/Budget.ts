import { Transaction } from './Transaction';

export type Budget = {
    BudgetId?: number;
    Amount: number;
    StartDate: Date;
    EndDate: Date;
    CategoryName: string;
    transactions?: Transaction[];
    TotalExpenses?: number;
    Rest?: number; 
}