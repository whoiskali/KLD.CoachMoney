using KLD.CoachMoney.Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLD.CoachMoney.Domain.Entities
{
    public sealed class Debt : AuditableEntity
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Creditor { get; private set; }
        public DebtType Type { get; private set; }
        public decimal OriginalAmount { get; private set; }
        public decimal CurrentBalance { get; private set; }
        public decimal? CreditorMinimumPayment { get; private set; }
        public DateTime DueDate { get; private set; }

        // EF Core only
        private Debt() { }

        // Domain constructor
        public Debt(
            Guid userId,
            string creditor,
            DebtType type,
            decimal originalAmount,
            DateTime dueDate)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Creditor = creditor;
            Type = type;
            OriginalAmount = originalAmount;
            CurrentBalance = originalAmount;
            DueDate = dueDate;
        }

        public void ApplyPayment(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Payment amount must be greater than zero.");

            CurrentBalance = Math.Max(0, CurrentBalance - amount);
        }
    }

    public enum DebtType
    {
        CreditCard,
        PersonalLoan,
        BNPL,
        MicroLoan,
        Other
    }
}
