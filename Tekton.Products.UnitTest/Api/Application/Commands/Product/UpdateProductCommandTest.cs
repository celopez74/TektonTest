using FluentValidation.TestHelper;
using Tekton.Products.Api.Application.Commands.Products;
using Xunit;

public class UpdateProductCommandValidatorTests
{
    private readonly UpdateProductCommandValidator _validator;

    public UpdateProductCommandValidatorTests()
    {
        _validator = new UpdateProductCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        // Arrange
        var command = new UpdateProductCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
              .WithErrorMessage("Product id is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var command = new UpdateProductCommand { Name = "" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Product name is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Status_Is_Invalid()
    {
        // Arrange
        var command = new UpdateProductCommand { Status = -1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Status)
              .WithErrorMessage("Product Status must be 0 or 1.");
    }

    [Fact]
    public void Should_Have_Error_When_Stock_Is_Negative()
    {
        // Arrange
        var command = new UpdateProductCommand { Stock = -1 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Stock)
              .WithErrorMessage("Product Stock must be greater than or equal to 0.");
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        // Arrange
        var command = new UpdateProductCommand { Description = "" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
              .WithErrorMessage("Product Description is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Price_Is_Less_Than_Or_Equal_To_Zero()
    {
        // Arrange
        var command = new UpdateProductCommand { Price = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price)
              .WithErrorMessage("Product Price must be greater than 0.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "Product A",
            Status = 1,
            Stock = 10,
            Description = "Sample description",
            Price = 100.0M
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
