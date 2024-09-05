using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using Tekton.Products.Infraestructure.Services;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using Tekton.Products.Domain.Models;
using Tekton.Products.Domain.SeedWork;
using Tekton.Products.Infraestructure.Services.MockApi;

namespace Tekton.Products.Infrastructure.Finder.Products
{
    public class ProductFinder : IProductFinder
    {
        private readonly TektonContext _context;
        private readonly IProductStateCacheService _cache;
        private readonly IDiscountService _discountService;

        public ProductFinder(TektonContext context, IProductStateCacheService cache,  IDiscountService discountService)
        {
            _context = context;
            _cache = cache;
            _discountService = discountService;
        }

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();
        
        /// <summary>
        /// Retrieves a ProductDto by its unique identifier asynchronously.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>A ProductDto object representing the product with additional details like discount and final price.</returns>  
        public async Task<ProductDto> GetProductDtoByIdAsync(Guid productId)
        {
            var product = await FindByIdAsync(productId);
            if (product == null)
                return null;

            var productDto = new ProductDto 
            {                                   
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Status = product.Status,
                StatusName = _cache.GetProductStatus(product.Status),
                Stock = product.Stock,
                Description = product.Description,                                    
            };            
            var discount = await _discountService.GetDiscountAsync(productId);
            productDto.Discount = discount;
            productDto.FinalPrice = product.Price * (100 - discount) / 100;

            return productDto;
        }  

        /// <summary>
        /// Asynchronously finds a product by its ID.
        /// </summary>
        /// <param name="productId">The ID of the product to find.</param>
        /// <returns>The product with the specified ID, or null if not found.</returns>
        public async Task<Product> FindByIdAsync(Guid productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return null;
            return product;
        }    
    }
}
