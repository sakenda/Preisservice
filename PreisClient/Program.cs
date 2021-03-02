using System;
using System.Collections.Generic;

namespace PreisClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(new string('=', 40) + " Test API Call");

            RequestModel model = new ApiCaller().GetAllEventData(3, 1015);
            Console.WriteLine(model.UserID.ToString() + model.ProductID.ToString() + model.PriceTotal.ToString());

            Console.WriteLine(new string('=', 40) + " Products");
            foreach (var item in DatabaseService.ProductList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine(new string('=', 40) + " Users");
            foreach (var item in DatabaseService.UserList)
            {
                Console.WriteLine(item);
            }
        }
    }
}