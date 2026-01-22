using System;
using System.Collections.Generic;
using System.Text;

namespace KLD.CoachMoney.Contracts.Debts
{
    public sealed class DebtDto
    {
        public Guid Id { get; init; }
        public string Creditor { get; init; } = null!;
        public decimal OriginalAmount { get; init; }
        public decimal CurrentBalance { get; init; }
        public decimal MinimumPayment { get; init; }
        public bool IsEstimatedMinimum { get; init; }
        public DateTime DueDate { get; init; }
        public string Type { get; init; } = null!;
    }
}
