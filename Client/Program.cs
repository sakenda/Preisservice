using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreisClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(new string('=', 40) + " Übersicht User");
            foreach (var item in DatabaseService.UserList)
            {
                Console.Write(item + ", ");
            }
            Console.WriteLine();
            Console.WriteLine(new string('=', 40) + " Übersicht Produktpreise");
            foreach (var item in DatabaseService.ProductList)
            {
                Console.Write($"{item,-10}");
            }
            Console.WriteLine();
            Console.WriteLine(new string('=', 40) + " Test API Call");
            RequestModel model;

            foreach (var user in DatabaseService.UserList)
            {
                Console.WriteLine("Kundennummer: " + user);
                foreach (var product in DatabaseService.ProductList)
                {
                    model = new ApiCaller().GetAllEventData(user, product);
                    Console.WriteLine("    Produktnummer: " + product + new string(' ', 15) + model.PriceTotal.ToString());
                }
            }

            Console.ReadKey();
        }
    }
}