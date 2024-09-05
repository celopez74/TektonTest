using LazyCache;
namespace Tekton.Products.Infraestructure.Services
{    
    public class ProductStateCacheService : IProductStateCacheService
    {
        private readonly IAppCache _cache;

        public ProductStateCacheService(IAppCache cache)
        {
            _cache = cache;
        }

        public Dictionary<int, string> GetProductStates()
        {
            
            string cacheKey = "productStates";
            
            return _cache.GetOrAdd(cacheKey, () =>
            {
                
                var states = new Dictionary<int, string>
                {
                    { 1, "Active" },
                    { 0, "Inactive" }
                };

                return states;
            }, TimeSpan.FromMinutes(5)); 
        }
        public string GetProductStatus(int status)
        {
            var states = GetProductStates();
            return states.ContainsKey(status) ? states[status] : "Unknown";
        }
    }
}