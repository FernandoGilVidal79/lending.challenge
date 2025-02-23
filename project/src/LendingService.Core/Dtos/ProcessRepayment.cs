namespace LendingService.Core.Dtos
{
    public class ProcessRepayment
    {
        public int Id { get; set; }

        public decimal BalaceLeft { get; set; }

        public DateTime DueDate { get; set; }
    }
}