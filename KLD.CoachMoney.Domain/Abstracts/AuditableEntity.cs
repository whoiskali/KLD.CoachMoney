using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLD.CoachMoney.Domain.Abstracts
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; protected set; }
        public Guid CreatedBy { get; protected set; }

        public DateTime? UpdatedAt { get; protected set; }
        public Guid? UpdatedBy { get; protected set; }

        internal void SetCreated(Guid userId, DateTime utcNow)
        {
            CreatedAt = utcNow;
            CreatedBy = userId;
        }

        internal void SetUpdated(Guid userId, DateTime utcNow)
        {
            UpdatedAt = utcNow;
            UpdatedBy = userId;
        }
    }
}
