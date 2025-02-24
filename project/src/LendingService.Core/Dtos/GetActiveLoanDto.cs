using LendingService.Core.Models;

namespace LendingService.Core.Dtos
{
    public class GetActiveLoanDto
    {

        public decimal BalanceLeft { get; set; }

        public DateTime DueDate { get; set; }

        public OfferDto Offer { get; set; }
    }
}
