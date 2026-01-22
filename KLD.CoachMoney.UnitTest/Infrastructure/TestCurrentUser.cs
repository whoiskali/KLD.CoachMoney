using KLD.CoachMoney.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLD.CoachMoney.UnitTest.Infrastructure
{
    public sealed class TestCurrentUser : ICurrentUser
    {
        public Guid UserId { get; }
        public string Username { get; }
        public string Name { get; }

        public TestCurrentUser()
        {
            UserId = Guid.NewGuid();
            Username = "juan.dela.cruz";
            Name = "Juan Dela Cruz";
        }
    }
}
