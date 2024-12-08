export type Transaction = 
{
    Id?:number;
    Amount : number;
    Description: string;
    TransactionDate?:Date| null;
    Type:string;
    CategoryName:string;
}