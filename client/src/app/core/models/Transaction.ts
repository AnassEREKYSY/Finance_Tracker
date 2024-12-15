export type Transaction = 
{
    Id?:number;
    amount : number;
    description: string;
    TransactionDate?:Date| null;
    type:string;
    categoryName:string;
}