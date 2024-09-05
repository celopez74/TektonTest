using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;

namespace Tekton.Products.Api.Application.Commands.Products
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Object>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductFinder _productFinder;

        public UpdateProductCommandHandler(IProductRepository productRepository, IProductFinder productFinder)
        {
            _productRepository = productRepository;
            _productFinder = productFinder; 
        }

        public async Task<Object> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var productToUpdate = await _productFinder.FindByIdAsync(request.Id);
                if (productToUpdate == null)
                {
                    throw new Exception("Product not found");   
                }
                productToUpdate.Name = request.Name;
                productToUpdate.Status = request.Status;
                productToUpdate.Stock = request.Stock;
                productToUpdate.Description = request.Description;
                productToUpdate.Price = request.Price;
                _productRepository.Update(productToUpdate);
                var saveOk = await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
               
                return saveOk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
