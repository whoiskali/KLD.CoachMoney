using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLD.CoachMoney.Domain.Entities
{
    public sealed class CoachSession
    {
        public Guid Id { get; private set; }
        public DateTime StartedAt { get; private set; }
        public string Topic { get; private set; }

        private readonly List<CoachMessage> _messages = new();
        public IReadOnlyCollection<CoachMessage> Messages => _messages.AsReadOnly();

        public CoachSession(string topic)
        {
            Id = Guid.NewGuid();
            StartedAt = DateTime.UtcNow;
            Topic = topic;
        }

        public void AddMessage(CoachMessage msg) => _messages.Add(msg);
    }

    public sealed class CoachMessage
    {
        public Guid Id { get; private set; }
        public CoachRole Role { get; private set; }
        public string Content { get; private set; }

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
