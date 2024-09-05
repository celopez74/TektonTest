 namespace Tekton.Products.Infraestructure.Services.MockApi
 {
    public interface IDiscountService
    {
        Task<int> GetDiscountAsync(Guid productId);
    }
 }