using System;
using System.Collections.Generic;

namespace PreisClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            RequestModel model;
            ApiCaller apiCaller = new ApiCaller();
            List<RequestModel> apiModels = new List<RequestModel>();

            Console.WriteLine(" API-Abfrage " + new string('=', 40));
            Console.WriteLine();

            foreach (var user in DatabaseService.UserList)
            {
                Console.WriteLine(" - Kundennummer: " + user);
                foreach (var product in DatabaseService.ProductList)
                {
                    model = apiCaller.CallUserProductPriceAsync(user, product);
                    Console.WriteLine($"    Produktnummer: {product,-15}Preis: {model.PriceTotal}");
                    apiModels.Add(model);
                }
                Console.WriteLine();
            }

            Console.WriteLine(" Ende-Abfrage " + new string('=', 39));

            Console.ReadKey();
        }
    }
}