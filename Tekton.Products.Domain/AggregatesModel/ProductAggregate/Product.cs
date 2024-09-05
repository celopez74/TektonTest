using Tekton.Products.Domain.SeedWork;

namespace Tekton.Products.Domain.AggregatesModel.ProductAggregate;

public class Product : Entity, IAggregateRoot
{   public string Name { get; set; }
    public int Status { get; set; } 
    public int Stock { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

}