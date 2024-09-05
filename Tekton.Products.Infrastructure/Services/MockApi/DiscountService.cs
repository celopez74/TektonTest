using System.Text.Json;
using Tekton.Products.Domain.Models;
using Microsoft.Extensions.Configuration;

 namespace Tekton.Products.Infraestructure.Services.MockApi
 {
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public DiscountService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<int> GetDiscountAsync(Guid productId)
        {
            var url = _configuration.GetValue<string>("Services:mockapi");
            url = $"{url}/product-discount/{productId.ToString()}";
            var response = await _httpClient.GetAsync(url);
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var contentBody = JsonSerializer.Deserialize<MockApiResponse>(responseContent);
                return contentBody== null? 0 :contentBody.discount;
            }
            else
            {
                return 0;
            }
            
        }
    }
 }