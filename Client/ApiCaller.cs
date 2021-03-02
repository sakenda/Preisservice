using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace PreisClient
{
    public class ApiCaller
    {
        public RequestModel GetAllEventData(int userId, int productId)
        {
            using (var client = new WebClient())
            {
                var result = client.DownloadString($"https://localhost:44378/price/db?userid={userId}&productid={productId}");
                return JsonConvert.DeserializeObject<RequestModel>(result);
            }
        }
    }
}