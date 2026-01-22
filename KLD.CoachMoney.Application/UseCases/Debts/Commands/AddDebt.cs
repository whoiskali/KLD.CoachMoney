using FluentValidation;
using KLD.CoachMoney.Application.Abstractions;
using KLD.CoachMoney.Application.Abstractions.Messaging;
using KLD.CoachMoney.Domain.Entities;

namespace KLD.CoachMoney.Application.UseCases.Debts.Commands;

public static class AddDebt
{
    // =========================
    // COMMAND
    // =========================
    public sealed class Command : ICommand
    {
        public string Creditor { get; init; } = null!;
        public DebtType Type { get; init; }
        public decimal Amount { get; init; }
        public DateTime DueDate { get; init; }
    }

    // =========================
    // VALIDATOR
    // =========================
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Creditor).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow.Date);
        }
    }

    // =========================
    // HANDLER
    // =========================
    public sealed class Handler(
            IValidator<Command> validator,
            IApplicationDbContext db,
            ICurrentUser currentUser) : ICommandHandler<Command>
    {
        public async Task HandleAsync(Command command, CancellationToken ct)
        {
            using var tr = await db.Database.BeginTransactionAsync();
            try
            {
                await validator.ValidateAndThrowAsync(command, ct);

                var debt = new Debt(
                    currentUser.UserId,
                    command.Creditor,
                    command.Type,
                    command.Amount,
                    command.DueDate
                );

                db.Debts.Add(debt);
                await db.SaveChangesAsync();
                await tr.CommitAsync();
            }
            catch (Exception)
            {
                await tr.RollbackAsync();
                throw;
            }
        }
    }
}
