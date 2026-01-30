using KLD.CoachMoney.Application.Abstractions.Auth;
using KLD.CoachMoney.Application.Abstractions.Messaging;
using KLD.CoachMoney.Contracts.Auth;

namespace KLD.CoachMoney.Application.UseCases.Auth.Commands;

public static class GenerateToken
{
    // =========================
    // COMMAND
    // =========================
    public sealed class Command : ICommand
    {
        public Guid UserId { get; init; }

        // Result populated by handler
        public AuthResultDto? Result { get; internal set; }
    }

    // =========================
    // HANDLER
    // =========================
    public sealed class Handler(
        ITokenService tokenService)
        : ICommandHandler<Command>
    {
        public Task HandleAsync(
            Command command,
            CancellationToken ct)
        {
            var token = tokenService.GenerateAccessToken(command.UserId);

            command.Result = new AuthResultDto
            {
                AccessToken = token,
            };

            return Task.CompletedTask;
        }
    }
}
