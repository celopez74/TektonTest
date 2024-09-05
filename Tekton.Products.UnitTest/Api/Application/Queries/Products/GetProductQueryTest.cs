using FluentValidation.TestHelper;
using Xunit;
using Tekton.Products.Api.Application.Queries.Products;
using static Tekton.Products.Api.Application.Queries.Products.GetProductQuery;

public class GetProductQueryValidatorTests
{
    private readonly GetProductQueryValidator _validator;

    public GetProductQueryValidatorTests()
    {
        _validator = new GetProductQueryValidator();
    }

    [Fact]
    public void ShouldHaveValidationErrorWhenIdIsEmpty()
    {
        // Arrange
        var query = new GetProductQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.id);
    }

    [Fact]
    public void ShouldNotHaveValidationErrorWhenIdIsValid()
    {
        // Arrange
        var query = new GetProductQuery(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.id);
    }
}
