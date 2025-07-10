using System;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class WriteAuditLogRepository : IWriteRepository<AuditLog, AuditLog>
    {
        private readonly ApplicationDbContext _context;

        public WriteAuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog entity)
        {
            // think the Details, EventType, EventTypeTargetId, Status and UserId will be captured on the controller.
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            await _context.AuditLogs.AddAsync(entity);
        }

        public Task UploadAsync(IEnumerable<AuditLog> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AuditLog entity)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
