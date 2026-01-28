using System;
using System.Linq.Expressions;
using Application.AuditLogs;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditLogsController : ControllerBase
{
    private readonly IReadRepository<AuditLog, ResponseAuditLogDto> _readRepository;

    public AuditLogsController(IReadRepository<AuditLog, ResponseAuditLogDto> readRepository)
    {
        _readRepository = readRepository;
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(List<ResponseAuditLogDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<List<ResponseAuditLogDto>>> GetAll()
    {
        try
        {
            var auditLogs = await _readRepository.GetAllAsync();
            return Ok(auditLogs);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResponseAuditLogDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<ResponseAuditLogDto>> GetById(Guid id)
    {
        try
        {
            var auditLog = await _readRepository.GetByIdAsync(id);
            if (auditLog == null)
            {
                return NotFound();
            }

            return Ok(ToResponse(auditLog));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }

    [HttpGet("find")]
    [ProducesResponseType(typeof(List<ResponseAuditLogDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<List<ResponseAuditLogDto>>> Find(
        [FromQuery] AuditLogEventType? eventType = null,
        [FromQuery] string? targetId = null,
        [FromQuery] string? userId = null,
        [FromQuery] DateTime? createdFrom = null,
        [FromQuery] DateTime? createdTo = null)
    {
        try
        {
            Expression<Func<AuditLog, bool>> predicate = log =>
                (!eventType.HasValue || log.EventType == eventType.Value) &&
                (string.IsNullOrWhiteSpace(targetId) || log.EventTypeTargetId == targetId) &&
                (string.IsNullOrWhiteSpace(userId) || log.UserId == userId) &&
                (!createdFrom.HasValue || log.CreatedAt >= createdFrom.Value) &&
                (!createdTo.HasValue || log.CreatedAt <= createdTo.Value);

            var auditLogs = await _readRepository.FindAsync(predicate);

            return Ok(auditLogs);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }

    private static ResponseAuditLogDto ToResponse(AuditLog auditLog)
    {
        return new ResponseAuditLogDto
        {
            Id = auditLog.Id,
            EventType = auditLog.EventType,
            Status = auditLog.Status,
            EventTypeTargetId = auditLog.EventTypeTargetId,
            Details = auditLog.Details,
            UserId = auditLog.UserId,
            CreatedAt = auditLog.CreatedAt
        };
    }
}
