namespace LendingService.Core.Dtos
{
    public class ProcessRepaymentDto
    {
        public int Id { get; set; }

        public decimal BalaceLeft { get; set; }

        public DateTime DueDate { get; set; }
    }
}