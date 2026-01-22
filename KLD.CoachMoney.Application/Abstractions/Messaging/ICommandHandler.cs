using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace KLD.CoachMoney.Application.Abstractions.Messaging
{
    public interface ICommandHandler<TCommand>
    where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, CancellationToken ct);
    }
}
