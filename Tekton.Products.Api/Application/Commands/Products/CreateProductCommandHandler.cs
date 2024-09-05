using MediatR;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using Tekton.Products.Domain.Exception;

namespace Tekton.Products.Api.Application.Commands.Products
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Object>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Object> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var productToCreate = new Product{
                    Name = request.Name,
                    Status = request.Status,
                    Stock = request.Stock,
                    Description = request.Description,
                    Price = request.Price
                };

                var productSaved = _productRepository.Add(productToCreate);
                var saveOk = await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                if (saveOk)
                {
                    return new
                    {                        
                        Id = productSaved.Id
                    };
                }
                else
                {
                    return saveOk;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
