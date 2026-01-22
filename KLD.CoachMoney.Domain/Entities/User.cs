using KLD.CoachMoney.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLD.CoachMoney.Domain.Entities
{
    public sealed class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string DisplayName { get; private set; }

        private readonly List<Debt> _debts = new();
        public IReadOnlyCollection<Debt> Debts => _debts.AsReadOnly();

        private readonly List<CoachSession> _sessions = new();
        public IReadOnlyCollection<CoachSession> Sessions => _sessions.AsReadOnly();

        public User(string email, string displayName)
        {
            Id = Guid.NewGuid();
            Email = email;
            DisplayName = displayName;
        }

        public void AddDebt(Debt debt) => _debts.Add(debt);
        public void AddSession(CoachSession session) => _sessions.Add(session);
    }

}
