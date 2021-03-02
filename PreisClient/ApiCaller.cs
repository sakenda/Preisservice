using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace PreisClient
{
    public class ApiCaller
    {
        public RequestModel GetAllEventData(int userId, int productId)
        {
            using (var client = new WebClient()) //WebClient
            {
                client.Headers.Add("Content-Type:application/json");
                client.Headers.Add("Accept:application/json");
                var result = client.DownloadString($"https://localhost:44378/price/db?userid={userId}&productid={productId}");
                var model = JsonSerializer.Deserialize<RequestModel>(result);
                return model;
            }
        }
    }
}