using FluentValidation;
using KLD.CoachMoney.Application.Abstractions;
using KLD.CoachMoney.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;

namespace KLD.CoachMoney.Application.UseCases.Debts.Commands;

public static class ApplyPayment
{
    // =========================
    // COMMAND
    // =========================
    public sealed record Command : ICommand
    {
        public Guid DebtId { get; init; }
        public decimal Amount { get; init; }
    }

    // =========================
    // VALIDATOR
    // =========================
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.DebtId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }

    // =========================
    // HANDLER
    // =========================
    public sealed class Handler(
            IValidator<Command> validator,
            IApplicationDbContext db,
            ICurrentUser currentUser)
        : ICommandHandler<Command>
    {
        public async Task HandleAsync(Command command, CancellationToken ct)
        {
            using var tr = await db.Database.BeginTransactionAsync(ct);
            try
            {
                await validator.ValidateAndThrowAsync(command, ct);

                var debt = await db.Debts
                    .Where(d =>
                        d.Id == command.DebtId &&
                        d.UserId == currentUser.UserId)
                    .SingleOrDefaultAsync(ct);

                if (debt is null)
                    throw new InvalidOperationException("Debt not found.");

                debt.ApplyPayment(command.Amount);

                await db.SaveChangesAsync(ct);
                await tr.CommitAsync(ct);
            }
            catch
            {
                await tr.RollbackAsync(ct);
                throw;
            }
        }
    }
}
