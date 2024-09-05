namespace Tekton.Products.Infraestructure.Services
{
    public interface IProductStateCacheService
    {
        Dictionary<int, string> GetProductStates();
        string GetProductStatus(int status);
    }
}