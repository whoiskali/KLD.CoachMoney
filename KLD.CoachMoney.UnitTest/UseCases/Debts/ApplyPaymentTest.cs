using FluentAssertions;
using KLD.CoachMoney.Application.UseCases.Debts.Commands;
using KLD.CoachMoney.Domain.Entities;
using KLD.CoachMoney.Domain.Enums;
using KLD.CoachMoney.UnitTest.Infrastructure;

namespace KLD.CoachMoney.UnitTest.UseCases.Debts;

public sealed class ApplyPaymentTests
{
    [Fact]
    public async Task HandleAsync_Should_apply_payment_and_create_audit()
    {
        // --------------------
        // Arrange
        // --------------------
        var currentUser = new TestCurrentUser();
        using var db = DbContextFactory.Create();

        var debt = new Debt(
            currentUser.UserId,
            "BPI Credit Card",
            DebtType.CreditCard,
            15_000m,
            DateTime.UtcNow.AddDays(30));

        db.Debts.Add(debt);
        await db.SaveChangesAsync();

        var validator = new ApplyPayment.Validator();

        var handler = new ApplyPayment.Handler(
            validator,
            db,
            currentUser
        );

        var command = new ApplyPayment.Command
        {
            DebtId = debt.Id,
            Amount = 5_000m
        };

        // --------------------
        // Act
        // --------------------
        await handler.HandleAsync(command, CancellationToken.None);

        // --------------------
        // Assert (Debt)
        // --------------------
        var updatedDebt = db.Debts.Single();

        updatedDebt.CurrentBalance.Should().Be(10_000m);

        // --------------------
        // Assert (AuditTrail)
        // --------------------
        var audit = db.AuditTrails
            .Where(a => a.Action == AuditAction.Modified)
            .Single();

        audit.TableName.Should().Be(nameof(Debt));
        audit.EntityId.Should().Be(debt.Id.ToString());
        audit.PerformedByName.Should().Be(currentUser.Name.ToString());
        audit.Changes.Should().NotBeNullOrWhiteSpace();
    }
}
