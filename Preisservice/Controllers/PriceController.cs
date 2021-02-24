using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace Preisservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PriceController : Controller
    {
        private readonly IDatabase _database;

        public PriceController(IDatabase service)
        {
            _database = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        // https://localhost:44378/price/db?userid=3&productid=1015
        // https://localhost:44378/price/db?userid=2&productid=2020
        [Route("DB")]
        [HttpGet]
        public object GetPriceByDB(int userID, int productID)
        {
            return StopwatchExecute(_database.GetProcessedProductPrices, productID, userID);
        }

        // https://localhost:44378/price/code?userid=3&productid=1015
        // https://localhost:44378/price/code?userid=2&productid=2020
        [Route("Code")]
        [HttpGet]
        public object GetPriceByService(int userID, int productID)
        {
            return StopwatchExecute(
                _database.GetRawProductPrices,
                new PriceCalculate().ProcessPrices,
                productID,
                userID);
        }

        private T StopwatchExecute<T>(Func<int, int, T> func, int productID, int userID)
        {
            T result = default(T);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (func != null)
            {
                result = func(productID, userID);
            }

            watch.Stop();
            Debug.WriteLine($"Processtime in {func.Method}: {watch.ElapsedMilliseconds}ms");

            return result;
        }

        private T StopwatchExecute<T, K>(Func<int, int, K> func1, Func<K, T> func2, int productID, int userID)
        {
            K temp;
            T result = default(T);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (func1 != null && func2 != null)
            {
                temp = func1(productID, userID);
                result = func2(temp);
            }

            watch.Stop();
            Debug.WriteLine($"Processtime in {func1.Method}: {watch.ElapsedMilliseconds}ms");

            return result;
        }
    }
}