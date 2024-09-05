using Tekton.Products.Domain.Models;
using Tekton.Products.Domain.SeedWork;

namespace Tekton.Products.Domain.AggregatesModel.ProductAggregate
{
    public interface IProductFinder : IRepository<ProductDto>
    {
        Task<ProductDto> GetProductDtoByIdAsync(Guid productId);
        Task<Product> FindByIdAsync(Guid productId);  
    }
}
   