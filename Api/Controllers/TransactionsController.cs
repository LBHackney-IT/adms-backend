using System;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    
    private readonly IReadRepository<Transaction, ResponseTransactionDto> _readRepository;
    private readonly IWriteRepository<Transaction, WriteTransactionDto> _writeRepository;
    
    public TransactionsController(
        IReadRepository<Transaction, ResponseTransactionDto> readRepository,
        IWriteRepository<Transaction, WriteTransactionDto> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }
    
    [HttpGet("all")]
    [ProducesResponseType(typeof(List<ResponseTransactionDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<List<ResponseTransactionDto>>> GetAll()
    {
        try
        {
            var transactions = await _readRepository.GetAllAsync();
            return Ok(transactions.ToList());
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
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseTransactionDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<Transaction>> GetById(Guid id)
    {
        try
        {
            var transaction = await _readRepository.GetByIdAsync(id);
            return Ok(transaction);
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
    
    [HttpGet("uln/{uln}")]
    [ProducesResponseType(typeof(List<ResponseTransactionDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetByUlnAsync(decimal uln)
    {
        try
        {
            var transactions = await _readRepository.GetByUlnAsync(uln);
            return Ok(transactions);
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
    [ProducesResponseType(typeof(List<ResponseTransactionDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<List<ResponseTransactionDto>>> Find(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] string? description = null
    )
    {
        try
        {
            System.Linq.Expressions.Expression<Func<Transaction, bool>> predicate = t =>
                (!fromDate.HasValue || t.TransactionDate >= fromDate.Value) &&
                (!toDate.HasValue || t.TransactionDate <= toDate.Value) &&
                (string.IsNullOrEmpty(description) || t.Description == description);

            var transactions = await _readRepository.FindAsync(predicate);

            return Ok(transactions);
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
    
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<WriteTransactionDto>> TransactionCreate(WriteTransactionDto transactionDto)
    {
        try
        {
            await _writeRepository.AddAsync(transactionDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }

    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<WriteTransactionDto>> TransactionUpload(IEnumerable<WriteTransactionDto> newTransaction)
    {
        try
        {
            await _writeRepository.UploadAsync(newTransaction);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }
    
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult> UpdateTransaction(Transaction transaction)
    {
        try
        {
            await _writeRepository.UpdateAsync(transaction);
            return NoContent();
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
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        try
        {
            await _writeRepository.RemoveAsync(id);
            return NoContent();
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
}