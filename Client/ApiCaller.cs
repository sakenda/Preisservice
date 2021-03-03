using Newtonsoft.Json;
using System.Net;

namespace PreisClient
{
    public class ApiCaller
    {
        private const string API_ADDRESS = "https://localhost:44378/price/";

        public RequestModel CallUserProductPriceAsync(int userId, int productId)
        {
            using (var client = new WebClient())
            {
                client.BaseAddress = API_ADDRESS;
                var result = client.DownloadString($"db?userid={userId}&productid={productId}");
                return JsonConvert.DeserializeObject<RequestModel>(result);
            }
        }
    }
}