using Tekton.Products.Domain.SeedWork;

namespace Tekton.Products.Domain.AggregatesModel.ProductAggregate;

public interface IProductRepository : IRepository<Product>
{
    Product Add(Product user);
    bool Update(Product user);
}
