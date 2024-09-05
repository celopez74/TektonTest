using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Tekton.Products.Api.Application.Commands.Products;
using Tekton.Products.Api.Application.Queries.Products;
using Tekton.Products.Api.Controllers.V1;
using Tekton.Products.Domain.Exception;
using Xunit;
using FluentAssertions;
using MediatR;
using Tekton.Products.Domain.Models;
using Tekton.Products.Api.SeedWork;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using Tekton.Products.Domain.SeedWork;
using System.Reflection;

namespace Tekton.Products.Api.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly ProductController _controller;
        private readonly Mock<IMediator> _mediatorMock;

        public ProductControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnSuccess_WhenProductIsFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productDto = new ProductDto { ProductId = productId, Name = "Test Product" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductQuery>(), default))
                         .ReturnsAsync(productDto);

            // Act
            var result = await _controller.GetProductById(productId) as OkObjectResult;
            var responseData = result?.Value as ResponseData;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            responseData.Should().NotBeNull();
            responseData.Code.Should().Be(200);
            responseData.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenProductIsNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductQuery>(), default))
                         .ThrowsAsync(new EntityNotFoundException());

            // Act
            var result = await _controller.GetProductById(productId) as NotFoundObjectResult;
            var responseData = result?.Value as ResponseData;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
            responseData.Should().NotBeNull();
            responseData.Code.Should().Be(404);
            responseData.Message.Should().Be("Exception of type 'Tekton.Products.Domain.Exception.EntityNotFoundException' was thrown.");
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnSuccess_WhenProductIsCreated()
        {
            // Arrange
            var command = new CreateProductCommand { Name = "New Product", Status = 1, Stock = 10, Description = "Description", Price = 99.99m };
            var resultDto = new { Id = Guid.NewGuid() };
            _mediatorMock.Setup(m => m.Send(command, default))
                         .ReturnsAsync(resultDto);

            // Act
            var result = await _controller.CreateProduct(command) as OkObjectResult;
            var responseData = result?.Value as ResponseData;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            responseData.Should().NotBeNull();
            responseData.Code.Should().Be(200);
            responseData.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var command = new CreateProductCommand { Name = "New Product", Status = 1, Stock = 10, Description = "Description", Price = 99.99m };
            _mediatorMock.Setup(m => m.Send(command, default))
                         .ThrowsAsync(new BadRequestException("Invalid data"));

            // Act
            var result = await _controller.CreateProduct(command) as BadRequestObjectResult;
            var responseData = result?.Value as ResponseData;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            responseData.Should().NotBeNull();
            responseData.Code.Should().Be(400);
            responseData.Message.Should().Be("Invalid data");
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnSuccess_WhenProductIsUpdated()
        {
            // Arrange
            var command = new UpdateProductCommand { Id = Guid.NewGuid(), Name = "Updated Product", Status = 1, Stock = 20, Description = "Updated Description", Price = 199.99m };
            var resultDto = new { Success = true };
            _mediatorMock.Setup(m => m.Send(command, default))
                         .ReturnsAsync(resultDto);

            // Act
            var result = await _controller.UpdateProduct(command) as OkObjectResult;
            var responseData = result?.Value as ResponseData;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            responseData.Should().NotBeNull();
            responseData.Code.Should().Be(200);
            responseData.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var command = new UpdateProductCommand { Id = Guid.NewGuid(), Name = "Updated Product", Status = 1, Stock = 20, Description = "Updated Description", Price = 199.99m };
            _mediatorMock.Setup(m => m.Send(command, default))
                         .ThrowsAsync(new BadRequestException("Update failed"));

            // Act
            var result = await _controller.UpdateProduct(command) as BadRequestObjectResult;
            var responseData = result?.Value as ResponseData;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
            responseData.Should().NotBeNull();
            responseData.Code.Should().Be(400);
            responseData.Message.Should().Be("Update failed");
        }

        [Fact]
        public async Task GetProductById_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var productDto = new ProductDto(); // Asegúrate de tener el modelo correcto aquí.

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductQuery>(), default(CancellationToken)))
                .ReturnsAsync(productDto);

            // Act
            var result = await _controller.GetProductById(uuid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);            
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenEntityNotFoundExceptionThrown()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductQuery>(), default))
                .ThrowsAsync(new EntityNotFoundException(Guid.NewGuid(),"Product not found"));

            // Act
            var result = await _controller.GetProductById(uuid);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);

        }

        [Fact]
        public async Task GetProductById_ShouldReturnBadRequest_WhenBadRequestExceptionThrown()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductQuery>(), default))
                .ThrowsAsync(new BadRequestException("Invalid request"));

            // Act
            var result = await _controller.GetProductById(uuid);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);            
        }

        [Fact]
        public async Task GetProductById_ShouldReturnBadRequest_WhenTecktonExceptionThrown()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductQuery>(), default))
                .ThrowsAsync(new TecktonException("Teckton error"));

            // Act
            var result = await _controller.GetProductById(uuid);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnBadRequest_WhenGeneralExceptionThrown()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductQuery>(), default))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.GetProductById(uuid);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnOk_WhenProductCreated()
        {
            // Arrange
            var productCommand = new CreateProductCommand
            {
                Name = "New Product",
                Price = 100
                
            };

            var productResult = new ProductDto {ProductId =Guid.NewGuid(),  Name = "New Product" };
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default(CancellationToken)))
                .ReturnsAsync(productResult);

            // Act
            var result = await _controller.CreateProduct(productCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);           
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnNotFound_WhenEntityNotFoundExceptionThrown()
        {
            // Arrange
            var productCommand = new CreateProductCommand { Name = "New Product", Price = 100 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default(CancellationToken)))
                .ThrowsAsync(new EntityNotFoundException(Guid.NewGuid(), "Product not found"));

            // Act
            var result = await _controller.CreateProduct(productCommand);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);           
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnBadRequest_WhenBadRequestExceptionThrown()
        {
            // Arrange
            var productCommand = new CreateProductCommand { Name = "New Product", Price = 100 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default(CancellationToken)))
                .ThrowsAsync(new BadRequestException("Invalid data"));

            // Act
            var result = await _controller.CreateProduct(productCommand);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);            
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var productCommand = new CreateProductCommand { Name = "New Product", Price = 100 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default(CancellationToken)))
                .ThrowsAsync(new Exception("Internal error"));

            // Act
            var result = await _controller.CreateProduct(productCommand);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);            
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnOk_WhenProductUpdated()
        {
            // Arrange
            var productCommand = new UpdateProductCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated Product",
                Price = 150                
            };

            var productResult = new ProductDto { ProductId = productCommand.Id, Name = "Updated Product" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), default(CancellationToken)))
                .ReturnsAsync(productResult);

            // Act
            var result = await _controller.UpdateProduct(productCommand);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);           
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNotFound_WhenEntityNotFoundExceptionThrown()
        {
            // Arrange
            var productCommand = new UpdateProductCommand { Id = Guid.NewGuid(), Name = "Updated Product", Price = 150 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), default(CancellationToken)))
                .ThrowsAsync(new EntityNotFoundException(Guid.NewGuid(), "Product not found"));

            // Act
            var result = await _controller.UpdateProduct(productCommand);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);            
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnBadRequest_WhenBadRequestExceptionThrown()
        {
            // Arrange
            var productCommand = new UpdateProductCommand { Id = Guid.NewGuid(), Name = "Updated Product", Price = 150 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), default(CancellationToken)))
                .ThrowsAsync(new BadRequestException("Invalid data"));

            // Act
            var result = await _controller.UpdateProduct(productCommand);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);            
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var productCommand = new UpdateProductCommand { Id = Guid.NewGuid(), Name = "Updated Product", Price = 150 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), default(CancellationToken)))
                .ThrowsAsync(new Exception("Internal error"));

            // Act
            var result = await _controller.UpdateProduct(productCommand);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);            
        }

        
    }
}
