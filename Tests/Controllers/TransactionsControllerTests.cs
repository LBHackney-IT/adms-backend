using System;
using Api.Controllers;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;


namespace Tests.Controllers;

public class TransactionsControllerTests
{
    private readonly Mock<IReadRepository<Transaction, ResponseTransactionDto>> _mockReadRepository;
    private readonly Mock<IWriteRepository<Transaction, WriteTransactionDto>> _mockWriteRepository;
    private readonly TransactionsController _controller;

    public TransactionsControllerTests()
    {
        _mockReadRepository = new Mock<IReadRepository<Transaction, ResponseTransactionDto>>();
        _mockWriteRepository = new Mock<IWriteRepository<Transaction, WriteTransactionDto>>();
        _controller = new TransactionsController(_mockReadRepository.Object, _mockWriteRepository.Object);
    }
    
    private ResponseTransactionDto CreateTransactionResponseDto(Guid id, string description)
    {
        return new ResponseTransactionDto
        {
            Id = id,
            TransactionDate = DateTime.UtcNow,
            Description = description,
            TransactionType = "Test Type",
            EnglishPercentage = 0,
            GovernmentContribution = 0,
            LevyDeclared = 0,
            PaidFromLevy = 0,
            PayrollMonth = DateTime.UtcNow,
            TenPercentageTopUp = 0,
            Total = 100.0m,
            YourContribution = 0,
            CourseLevel = 1
        };
    }
    
    private WriteTransactionDto CreateTransactionWriteDto(DateTime transactionDate, string description)
    {
        return new WriteTransactionDto
        {
            TransactionDate = transactionDate,
            Description = description,
            TransactionType = "Test Type",
            EnglishPercentage = 0,
            GovernmentContribution = 0,
            LevyDeclared = 0,
            PaidFromLevy = 0,
            PayrollMonth = DateTime.UtcNow,
            TenPercentageTopUp = 0,
            Total = 100.0m,
            YourContribution = 0,
            CourseLevel = 1
        };
    }
    
    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfTransactions()
    {
        // Arrange
        var transactions = new List<ResponseTransactionDto>
        {
            CreateTransactionResponseDto(Guid.NewGuid(), "Transaction 1"),
            CreateTransactionResponseDto(Guid.NewGuid(), "Transaction 2")
        };
        _mockReadRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(transactions);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<ResponseTransactionDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedTransactions = Assert.IsAssignableFrom<IEnumerable<ResponseTransactionDto>>(okResult.Value);
        Assert.Equal(2, returnedTransactions.Count());
    }

    
    [Fact]
    public async Task GetById_ReturnsOkResult_WithTransaction()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var transaction = new Transaction
        {
            Id = transactionId,
            Description = "Test Transaction",
            TransactionDate = DateTime.UtcNow,
            TransactionType = "Test Type",
            EnglishPercentage = 0,
            GovernmentContribution = 0,
            LevyDeclared = 0,
            PaidFromLevy = 0,
            PayrollMonth = DateTime.UtcNow,
            TenPercentageTopUp = 0,
            Total = 150.0m,
            YourContribution = 0,
            CourseLevel = 1,
            CreatedAt = DateTime.UtcNow
        };
        _mockReadRepository.Setup(repo => repo.GetByIdAsync(transactionId)).ReturnsAsync(transaction);

        // Act
        var result = await _controller.GetById(transactionId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Transaction>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedTransaction = Assert.IsType<Transaction>(okResult.Value);
        Assert.Equal(transactionId, returnedTransaction.Id);
    }

    
    [Fact]
    public async Task Find_ReturnsOkResult_WithListOfTransactions()
    {
        // Arrange
        var request = new FindTransaction();
        var transactions = new List<ResponseTransactionDto>
        {
            CreateTransactionResponseDto(Guid.NewGuid(), "Transaction 1"),
            CreateTransactionResponseDto(Guid.NewGuid(), "Transaction 2")
        };
        _mockReadRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Transaction, bool>>>())).ReturnsAsync(transactions);

        // Act
        var result = await _controller.Find(request);

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<ResponseTransactionDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedTransactions = Assert.IsType<List<ResponseTransactionDto>>(okResult.Value);
        Assert.Equal(2, returnedTransactions.Count);
    }


    [Fact]
    public async Task TransactionCreate_ReturnsNoContent()
    {
        // Arrange
        var transactionDto = CreateTransactionWriteDto(DateTime.UtcNow, "New Transaction");
        _mockWriteRepository.Setup(repo => repo.AddAsync(transactionDto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.TransactionCreate(transactionDto);

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }


    [Fact]
    public async Task TransactionUpload_ReturnsNoContent()
    {
        // Arrange
        var transactionDtos = new List<WriteTransactionDto>
        {
            CreateTransactionWriteDto(DateTime.UtcNow, "Transaction 1"),
            CreateTransactionWriteDto(DateTime.UtcNow, "Transaction 2")
        };
        _mockWriteRepository.Setup(repo => repo.UploadAsync(transactionDtos)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.TransactionUpload(transactionDtos);

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }

    
    [Fact]
    public async Task UpdateTransaction_ReturnsNoContent()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = "Updated Transaction",
            TransactionDate = DateTime.UtcNow,
            TransactionType = "Test Type",
            EnglishPercentage = 0,
            GovernmentContribution = 0,
            LevyDeclared = 0,
            PaidFromLevy = 0,
            PayrollMonth = DateTime.UtcNow,
            TenPercentageTopUp = 0,
            Total = 200.0m,
            YourContribution = 0,
            CourseLevel = 1,
            CreatedAt = DateTime.UtcNow
        };
        _mockWriteRepository.Setup(repo => repo.UpdateAsync(transaction)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateTransaction(transaction);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    
    [Fact]
    public async Task DeleteTransaction_ReturnsNoContent()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        _mockWriteRepository.Setup(repo => repo.RemoveAsync(transactionId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteTransaction(transactionId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

}