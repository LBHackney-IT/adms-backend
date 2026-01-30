using System;
using System.Linq.Expressions;
using Application.AuditLogs;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReadAuditLogRepository : IReadRepository<AuditLog, ResponseAuditLogDto>
    {
        private readonly ApplicationDbContext _context;

        public ReadAuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ResponseAuditLogDto>> GetAllAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(a => a.CreatedAt)
                .Select(MapToResponse())
                .ToListAsync();
        }

        public async Task<AuditLog> GetByIdAsync(Guid id)
        {
            return await _context.AuditLogs.FindAsync(id);
        }

        public Task<List<ResponseAuditLogDto>> GetByUlnAsync(decimal uln)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResponseAuditLogDto>> FindAsync(Expression<Func<AuditLog, bool>> predicate)
        {
            return await _context.AuditLogs
                .Where(predicate)
                .OrderByDescending(a => a.CreatedAt)
                .Select(MapToResponse())
                .ToListAsync();
        }

        private static Expression<Func<AuditLog, ResponseAuditLogDto>> MapToResponse()
        {
            return auditLog => new ResponseAuditLogDto
            {
                Id = auditLog.Id,
                EventType = auditLog.EventType,
                Status = auditLog.Status,
                EntityName = auditLog.EntityName,
                EventTypeTargetId = auditLog.EventTypeTargetId,
                Details = auditLog.Details,
                UserId = auditLog.UserId,
                CreatedAt = auditLog.CreatedAt,
                CorrelationId = auditLog.CorrelationId
            };
        }
    }
}
