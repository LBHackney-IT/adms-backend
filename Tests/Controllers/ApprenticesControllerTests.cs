using System;
using System.Linq.Expressions;
using Api.Controllers;
using Application.Apprentices;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace Tests.Controllers
{
    public class ApprenticesControllerTests
    {
        private readonly Mock<IReadRepository<Apprentice, ResponseApprenticeDto>> _mockReadRepository;
        private readonly Mock<IWriteRepository<Apprentice, WriteApprenticeDto>> _mockWriteRepository;
        private readonly ApprenticesController _controller;

        public ApprenticesControllerTests()
        {
            _mockReadRepository = new Mock<IReadRepository<Apprentice, ResponseApprenticeDto>>();
            _mockWriteRepository = new Mock<IWriteRepository<Apprentice, WriteApprenticeDto>>();
            _controller = new ApprenticesController(_mockReadRepository.Object, _mockWriteRepository.Object);
        }

        private ResponseApprenticeDto CreateResponseApprenticeDto(Guid id, decimal uln)
        {
            return new ResponseApprenticeDto
            {
                Id = id,
                Name = "Test Apprentice",
                StartDate = DateTime.Now,
                Status = "Active",
                ULN = uln
            };
        }

        private WriteApprenticeDto CreateWriteApprenticeDto(decimal uln)
        {
            return new WriteApprenticeDto
            {
                Name = "Test Apprentice",
                StartDate = DateTime.Now,
                Status = "Active",
                ULN = uln
            };
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfApprentices()
        {
            // Arrange
            var apprentices = new List<ResponseApprenticeDto>
            {
                CreateResponseApprenticeDto(Guid.NewGuid(), 1234567890),
                CreateResponseApprenticeDto(Guid.NewGuid(), 1234567891)
            };
        
            _mockReadRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(apprentices);
        
            // Act
            var result = await _controller.GetAll();
        
            // Assert
            var actionResult = Assert.IsType<ActionResult<List<ResponseApprenticeDto>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedApprentices = Assert.IsAssignableFrom<IEnumerable<ResponseApprenticeDto>>(okResult.Value);
            Assert.Equal(2, returnedApprentices.Count());
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithApprentice()
        {
            // Arrange
            var apprenticeId = Guid.NewGuid();
            var apprentice = new Apprentice
            {
                Id = apprenticeId,
                Name = "Test Apprentice",
                StartDate = DateTime.Now,
                Status = "Active",
                ULN = 1234567890
            };
            _mockReadRepository.Setup(repo => repo.GetByIdAsync(apprenticeId)).ReturnsAsync(apprentice);

            // Act
            var result = await _controller.GetById(apprenticeId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Apprentice>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedApprentice = Assert.IsType<Apprentice>(okResult.Value);
            Assert.Equal(apprenticeId, returnedApprentice.Id);
        }

        [Fact]
        public async Task GetByUln_ReturnsOkResult_WithApprentice()
        {
            // Arrange
            var uln = 1234567890;
            var apprentice = new List<ResponseApprenticeDto> { CreateResponseApprenticeDto(Guid.NewGuid(), uln) };
            _mockReadRepository.Setup(repo => repo.GetByUlnAsync(uln)).ReturnsAsync(apprentice);

            // Act 
            var result = await _controller.GetByUlnAsync(uln);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Apprentice>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedApprentice = Assert.IsAssignableFrom<IEnumerable<ResponseApprenticeDto>>(okResult.Value).ToList();
            Assert.Equal(uln, returnedApprentice.First().ULN);
        }

        [Fact]
        public async Task Find_ReturnsOkResult_WithListOfApprentices()
        {
            // Arrange
            var request = new FindApprentice();
            var apprentices = new List<ResponseApprenticeDto>
            {
                CreateResponseApprenticeDto(Guid.NewGuid(), 1234567890),
                CreateResponseApprenticeDto(Guid.NewGuid(), 1234567891)
            };
            _mockReadRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Apprentice, bool>>>())).ReturnsAsync(apprentices);

            // Act
            var result = await _controller.Find(request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<ResponseApprenticeDto>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedApprentices = Assert.IsType<List<ResponseApprenticeDto>>(okResult.Value);
            Assert.Equal(2, returnedApprentices.Count);
        }

        [Fact]
        public async Task ApprenticeCreate_ReturnsNoContent()
        {
            // Arrange
            var apprenticeDto = CreateWriteApprenticeDto(1234567890);
            _mockWriteRepository.Setup(repo => repo.AddAsync(apprenticeDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ApprenticeCreate(apprenticeDto);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task ApprenticeUpload_ReturnsNoContent()
        {
            // Arrange
            var apprenticeDtos = new List<WriteApprenticeDto>
            {
                CreateWriteApprenticeDto(1234567890),
                CreateWriteApprenticeDto(1234567891)
            };
            _mockWriteRepository.Setup(repo => repo.UploadAsync(apprenticeDtos)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ApprenticeUpload(apprenticeDtos);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task UpdateApprentice_ReturnsNoContent()
        {
            // Arrange
            var apprentice = new Apprentice
            {
                Id = Guid.NewGuid(),
                Name = "Test Apprentice",
                StartDate = DateTime.Now,
                Status = "Active",
                ULN = 1234567890
            };
            _mockWriteRepository.Setup(repo => repo.UpdateAsync(apprentice)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateApprentice(apprentice);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteApprentice_ReturnsNoContent()
        {
            // Arrange
            var apprenticeId = Guid.NewGuid();
            _mockWriteRepository.Setup(repo => repo.RemoveAsync(apprenticeId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteApprentice(apprenticeId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}