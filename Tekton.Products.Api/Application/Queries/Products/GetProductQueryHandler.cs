using MediatR;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using Tekton.Products.Domain.Models;

namespace Tekton.Products.Api.Application.Queries.Products
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IProductFinder _productFinder;

        public GetProductQueryHandler(IProductFinder productFinder)
        {
            _productFinder = productFinder;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var productTekton = await _productFinder.GetProductDtoByIdAsync(request.id);
            return productTekton;
        }
    }
}
