using System;
using System.Collections.Generic;
using System.Text;

namespace KLD.CoachMoney.Application.Abstractions.Messaging
{
    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query, CancellationToken ct);
    }
}

