using FluentAssertions;
using KLD.CoachMoney.Application.UseCases.Debts.Commands;
using KLD.CoachMoney.Domain.Entities;
using KLD.CoachMoney.Domain.Enums;
using KLD.CoachMoney.UnitTest.Infrastructure;

namespace KLD.CoachMoney.UnitTest.UseCases.Debts
{
    public sealed class AddDebtTests
    {
        [Fact]
        public async Task HandleAsync_Should_create_debt_for_current_user()
        {
            // --------------------
            // Arrange
            // --------------------
            var currentUser = new TestCurrentUser();
            using var db = DbContextFactory.Create();

            var validator = new AddDebt.Validator();

            var handler = new AddDebt.Handler(
                validator,
                db,
                currentUser
            );

            var command = new AddDebt.Command
            {
                Creditor = "BPI Credit Card",
                Type = DebtType.CreditCard,
                Amount = 15000,
                DueDate = DateTime.UtcNow.AddDays(30)
            };

            // --------------------
            // Act
            // --------------------
            await handler.HandleAsync(command, CancellationToken.None);

            // --------------------
            // Assert
            // --------------------
            var debt = db.Debts.Single();

            debt.UserId.Should().Be(currentUser.UserId);
            debt.Creditor.Should().Be("BPI Credit Card");
            debt.OriginalAmount.Should().Be(15000);
            debt.CurrentBalance.Should().Be(15000);

            // --------------------
            // Assert (AuditTrail)
            // --------------------
            var audit = db.AuditTrails.Single();

            audit.TableName.Should().Be(nameof(Debt));
            audit.Action.Should().Be(AuditAction.Added);
            audit.EntityId.Should().Be(debt.Id.ToString());
            audit.PerformedByName.Should().Be(currentUser.Name.ToString());

            audit.Changes.Should().NotBeNullOrWhiteSpace();
        }
    }
}
