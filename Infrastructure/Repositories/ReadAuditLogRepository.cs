using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReadAuditLogRepository : IReadRepository<AuditLog, AuditLog>
    {
        private readonly ApplicationDbContext _context;

        public ReadAuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<AuditLog> GetByIdAsync(Guid id)
        {
            return await _context.AuditLogs.FindAsync(id);
        }

        public Task<List<AuditLog>> GetByUlnAsync(decimal uln)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AuditLog>> FindAsync(Expression<Func<AuditLog, bool>> predicate)
        {
            return await _context.AuditLogs
                .Where(predicate)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}
