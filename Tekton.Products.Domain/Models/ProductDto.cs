using Tekton.Products.Domain.SeedWork;

namespace Tekton.Products.Domain.Models;

public class ProductDto : IDto
{
    public Guid? ProductId { get; set; }
    public string Name { get; set; }
    public int Status { get; set; } 
    public string StatusName { get; set; } = string.Empty;
    public int Stock { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int? Discount { get; set; } = 0;
    public decimal FinalPrice { get; set; }
}

public class ProductIdDto : IDto
{
    public Guid Id { get; set; }
}