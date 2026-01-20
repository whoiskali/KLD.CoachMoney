using Company.Template.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Template.Domain.Entities
{
    public class AuditTrail
    {
        public long Id { get; set; }

        // Target entity info
        public string TableName { get; set; } = default!;
        public string EntityId { get; set; } = default!;

        // Action performed
        public AuditAction Action { get; set; }

        public string? Changes { get; set; }      // JSON

        // Metadata
        public string PerformedById { get; set; } = default!;
        public string PerformedByName { get; set; } = default!;
        public DateTimeOffset Timestamp { get; set; }
    }

}
