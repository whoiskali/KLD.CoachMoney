using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLD.CoachMoney.Domain.Entities
{
    public class ExceptionLog
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string ExceptionMessage { get; set; }
        public string? InnerException { get; set; }
        public string? StackTrace { get; set; }
        public DateTime OccurredAt { get; set; }
        public string? Source { get; set; }
    }
}
