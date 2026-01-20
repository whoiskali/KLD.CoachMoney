using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Company.Template.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Template.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }

        public DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void UpdateRange([NotNullAttribute] IEnumerable<object> entities);
        EntityEntry Update([NotNullAttribute] object entity);
    }
}
