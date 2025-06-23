using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Infrastructure.Repositories;

public class WriteTransactionRepository: IWriteRepository<Transaction>
{
    
    private readonly ApplicationDbContext _context;

    public WriteTransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Transaction entity)
    {
        await _context.Transactions.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Transaction> entities)
    {
        await _context.Transactions.AddRangeAsync(entities);
        await _context.SaveChangesAsync();

    }

    public async Task UpdateAsync(Transaction entity)
    {
        var transaction = _context.Transactions.Find(entity.Id);
        if(transaction != null)
            _context.Entry(transaction).CurrentValues.SetValues(entity);
        await  _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var transaction = _context.Transactions.Find(id);
        if(transaction != null)
            _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }
}