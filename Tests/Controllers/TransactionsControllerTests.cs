
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
            CourseLevel = 1,
            CreatedAt = DateTime.UtcNow,
            ULN = 1234567890
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
            CourseLevel = 1,
            ULN = 1234567890
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
            CreatedAt = DateTime.UtcNow,
            ULN = 1234567890
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
        var transactions = new List<ResponseTransactionDto>
        {
            CreateTransactionResponseDto(Guid.NewGuid(), "Transaction 1"),
            CreateTransactionResponseDto(Guid.NewGuid(), "Transaction 2")
        };
        _mockReadRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Transaction, bool>>>())).ReturnsAsync(transactions);

        // Act
        var result = await _controller.Find();

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<ResponseTransactionDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedTransactions = Assert.IsType<List<ResponseTransactionDto>>(okResult.Value);
        Assert.Equal(2, returnedTransactions.Count);
    }

    [Fact]
    public async Task Find_WithParameters_ReturnsOkResult_WithListOfTransactions()
    {
        // Arrange
        var fromDate = DateTime.UtcNow.AddDays(-30);
        var toDate = DateTime.UtcNow;
        var description = "Test Transaction";
        var transactions = new List<ResponseTransactionDto>
        {
            CreateTransactionResponseDto(Guid.NewGuid(), "Test Transaction")
        };
        _mockReadRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Transaction, bool>>>())).ReturnsAsync(transactions);

        // Act
        var result = await _controller.Find(fromDate, toDate, description);

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<ResponseTransactionDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedTransactions = Assert.IsType<List<ResponseTransactionDto>>(okResult.Value);
        Assert.Equal(1, returnedTransactions.Count);
    }

    [Fact]
    public async Task Find_ThrowsException_ReturnsBadRequest()
    {
        // Arrange
        _mockReadRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Transaction, bool>>>())).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.Find();

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<ResponseTransactionDto>>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
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
            CreatedAt = DateTime.UtcNow,
            ULN = 1234567890
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
    
    [Fact]
    public async Task GetAll_ThrowsException_ReturnsBadRequest()
    {
        // Arrange
        _mockReadRepository.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetAll();

        // Assert
        var actionResult = Assert.IsType<ActionResult<List<ResponseTransactionDto>>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetById_ThrowsKeyNotFoundException_ReturnsNotFound()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        _mockReadRepository.Setup(repo => repo.GetByIdAsync(transactionId)).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.GetById(transactionId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Transaction>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetByUln_ReturnsOkResult_WithTransactions()
    {
        // Arrange
        var uln = 1234567890;
        var transactions = new List<ResponseTransactionDto> 
        { 
            CreateTransactionResponseDto(Guid.NewGuid(), "Transaction for ULN") 
        };
        _mockReadRepository.Setup(repo => repo.GetByUlnAsync(uln)).ReturnsAsync(transactions);

        // Act
        var result = await _controller.GetByUlnAsync(uln);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Transaction>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnedTransactions = Assert.IsAssignableFrom<IEnumerable<ResponseTransactionDto>>(okResult.Value).ToList();
        Assert.Equal(uln, returnedTransactions.First().ULN);
    }

    [Fact]
    public async Task GetByUln_ThrowsKeyNotFoundException_ReturnsNotFound()
    {
        // Arrange
        var uln = 1234567890;
        _mockReadRepository.Setup(repo => repo.GetByUlnAsync(uln)).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.GetByUlnAsync(uln);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Transaction>>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task TransactionCreate_ThrowsException_ReturnsBadRequest()
    {
        // Arrange
        var transactionDto = CreateTransactionWriteDto(DateTime.UtcNow, "New Transaction");
        _mockWriteRepository.Setup(repo => repo.AddAsync(transactionDto)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.TransactionCreate(transactionDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<WriteTransactionDto>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task TransactionUpload_ThrowsException_ReturnsBadRequest()
    {
        // Arrange
        var transactionDtos = new List<WriteTransactionDto>
        {
            CreateTransactionWriteDto(DateTime.UtcNow, "Transaction 1"),
            CreateTransactionWriteDto(DateTime.UtcNow, "Transaction 2")
        };
        _mockWriteRepository.Setup(repo => repo.UploadAsync(transactionDtos)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.TransactionUpload(transactionDtos);

        // Assert
        var actionResult = Assert.IsType<ActionResult<WriteTransactionDto>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task UpdateTransaction_ThrowsKeyNotFoundException_ReturnsNotFound()
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
            CreatedAt = DateTime.UtcNow,
            ULN = 1234567890
        };
        _mockWriteRepository.Setup(repo => repo.UpdateAsync(transaction)).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.UpdateTransaction(transaction);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteTransaction_ThrowsKeyNotFoundException_ReturnsNotFound()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        _mockWriteRepository.Setup(repo => repo.RemoveAsync(transactionId)).ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.DeleteTransaction(transactionId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}