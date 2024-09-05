using Microsoft.EntityFrameworkCore;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using Tekton.Products.Domain.SeedWork;
using Tekton.Products.Infrastructure.Finder.Products;

namespace Tekton.Products.Infrastructure.Repository;

public class ProductRepository : IProductRepository
{

    private readonly TektonContext _context;
    public IUnitOfWork UnitOfWork => _context;
    public ProductRepository(TektonContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));               
    }
    /// <summary>
    /// Adds a new product to the database.
    /// </summary>
    /// <param name="product">The product to be added.</param>
    /// <returns>The added product entity.</returns>
    public Product Add(Product product)
    {
        if (product == null) {
            throw new ArgumentNullException();
        }        
        return _context.Products.Add(product).Entity;
    }
    /// <summary>
    /// Updates an existing product in the database.
    /// </summary>
    /// <param name="product">The product to be updated.</param>
    /// <returns>True if the update is successful.</returns>
    public bool Update(Product product)
    {
        if (product == null) {
            throw new ArgumentNullException();
        }  
        _context.Entry(product).State = EntityState.Modified;
        _context.SaveChanges();
        return true;
    }   
}