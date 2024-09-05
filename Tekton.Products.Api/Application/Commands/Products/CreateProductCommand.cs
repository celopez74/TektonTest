using FluentValidation;
using MediatR;

namespace Tekton.Products.Api.Application.Commands.Products
{
    public class CreateProductCommand : IRequest<Object>    {
        
        public string Name { get; set; }
        public int Status { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
    
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {           
            RuleFor(command => command.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(command => command.Status).InclusiveBetween(0, 1).WithMessage("Product Status must be 0 or 1.");
            RuleFor(command => command.Stock).GreaterThanOrEqualTo(0).WithMessage("Product Stock must be greater than or equal to 0.");
            RuleFor(command => command.Description).NotEmpty().WithMessage("Product Description is required.");
            RuleFor(command => command.Price).GreaterThan(0).WithMessage("Product Price must be greater than 0.");
        }
    }
}
