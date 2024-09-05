using FluentValidation;
using MediatR;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using Tekton.Products.Domain.Models;

namespace Tekton.Products.Api.Application.Queries.Products
{
    public class GetProductQuery : IRequest<ProductDto>
    {
        public Guid id { get; set; }

        public GetProductQuery(Guid productId)
        {
            id = productId;
        }

        public class GetProductQueryValidator: AbstractValidator<GetProductQuery>
        {
            public GetProductQueryValidator() 
            { 
                RuleFor(x => x.id).NotEmpty();
            }
        }
    }
}
