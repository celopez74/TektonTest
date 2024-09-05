using Tekton.Products.Api.Application.Commands.Products;
using Xunit;
using FluentValidation.TestHelper;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator;

    public CreateProductCommandValidatorTests()
    {
        _validator = new CreateProductCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateProductCommand { Name = "" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Name).WithErrorMessage("Product name is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Status_Is_Out_Of_Range()
    {
        var command = new CreateProductCommand { Status = 2 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Status).WithErrorMessage("Product Status must be 0 or 1.");
    }

    [Fact]
    public void Should_Have_Error_When_Stock_Is_Less_Than_Zero()
    {
        var command = new CreateProductCommand { Stock = -1 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Stock).WithErrorMessage("Product Stock must be greater than or equal to 0.");
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var command = new CreateProductCommand { Description = "" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Description).WithErrorMessage("Product Description is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Price_Is_Less_Than_Or_Equal_To_Zero()
    {
        var command = new CreateProductCommand { Price = 0 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Price).WithErrorMessage("Product Price must be greater than 0.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new CreateProductCommand
        {
            Name = "Product A",
            Status = 1,
            Stock = 10,
            Description = "Sample description",
            Price = 100.0M
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}