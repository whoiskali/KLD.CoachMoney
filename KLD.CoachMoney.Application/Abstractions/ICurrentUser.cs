using System;
using System.Collections.Generic;
using System.Text;

namespace KLD.CoachMoney.Application.Abstractions
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        string Username { get; }
        string Name { get; }
    }
}
