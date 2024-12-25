namespace Core.Dtos
{
    public class BudgetReport
    {
        public string BudgetName { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal RemainingAmount => BudgetAmount - SpentAmount;
        public List<CreateUpdateTransaction> Transactions { get; set; } = new();
    }
}