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
    public async Task<ActionResult<List<ResponseTransactionDto>>> GetAll()
    {
        return await _readRepository.GetAllAsync();
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseTransactionDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Transaction>> GetById(Guid id)
    {
        var transaction = await _readRepository.GetByIdAsync(id);
        if (transaction == null)
        {
            return NotFound();
        }

        return Ok(transaction);
    }
    
    [HttpGet("uln/{uln}")]
    [ProducesResponseType(typeof(IEnumerable<Transaction>), 200)]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetByUlnAsync(decimal uln)
    {
        var transactions = await _readRepository.GetByUlnAsync(uln);
        return Ok(transactions);
    }
    
    [HttpPost("find")]
    [ProducesResponseType(typeof(List<ResponseTransactionDto>), 200)]
    public async Task<ActionResult<List<ResponseTransactionDto>>> Find(
        [FromBody] FindTransaction request
    )
    {
        System.Linq.Expressions.Expression<Func<Transaction, bool>> predicate = t =>
            (!request.FromDate.HasValue || t.TransactionDate >= request.FromDate.Value) &&
            (!request.ToDate.HasValue || t.TransactionDate <= request.ToDate.Value) &&
            (string.IsNullOrEmpty(request.Description) || t.Description == request.Description);

        var transactions = await _readRepository.FindAsync(predicate);

        return Ok(transactions);
    }
    
    [HttpPost("create")]
    [ProducesResponseType(typeof(Transaction), 201)]
    public async Task<ActionResult<WriteTransactionDto>> TransactionAdd(WriteTransactionDto transactionDto)
    {
        await _writeRepository.AddAsync(transactionDto);
        return Ok();
    }

    [HttpPost("create-range")]
    [ProducesResponseType(typeof(Transaction), 200)]
    public async Task<ActionResult<WriteTransactionDto>> BulkTransactionAdd(IEnumerable<WriteTransactionDto> newTransaction)
    {
        await _writeRepository.AddRangeAsync(newTransaction);
        return Ok();
    }
    
    [HttpPatch]
    [ProducesResponseType(typeof(Transaction), 200)]
    public async Task<ActionResult> UpdateTransaction(Transaction transaction)
    {
        await _writeRepository.UpdateAsync(transaction);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Transaction), 200)]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        await _writeRepository.RemoveAsync(id);
        return NoContent();
    }
}