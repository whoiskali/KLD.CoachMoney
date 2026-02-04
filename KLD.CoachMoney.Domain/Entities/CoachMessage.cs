using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLD.CoachMoney.Domain.Entities
{
    public sealed class CoachMessage
    {
        public Guid Id { get; private set; }
        public Guid CoachSessionId { get; private set; }
        public CoachRole Role { get; private set; }
        public string Content { get; private set; }

        public CoachSession CoachSession { get; private set; } = null!;

        public CoachMessage(CoachRole role, string content)
        {
            Id = Guid.NewGuid();
            Role = role;
            Content = content;
        }
    }

    public enum CoachRole
    {
        User,
        Coach
    }
}
