using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PreisService.Controllers
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

        [Route("DB")]
        [HttpGet]
        public async Task<UserPriceModel> GetPriceByDB(int userID, int productID)
        {
            // https://localhost:44378/price/db?userid=3&productid=1015
            // https://localhost:44378/price/db?userid=7&productid=2020

            Stopwatch watch = new Stopwatch();
            watch.Start();

            var result = await _database.GetProcessedProductPricesAsync(productID, userID);

            watch.Stop();
            Debug.WriteLine($"Processtime in DB: {watch.ElapsedMilliseconds}ms");

            return result;
        }

        [Route("Code")]
        [HttpGet]
        public async Task<UserPriceModel> GetPriceByService(int userID, int productID)
        {
            // https://localhost:44378/price/code?userid=3&productid=1015
            // https://localhost:44378/price/code?userid=7&productid=2020

            Stopwatch watch = new Stopwatch();
            watch.Start();

            var proxy = await _database.GetRawProductPricesAsync(productID, userID);
            var result = new PriceCalculate().ProcessPrices(proxy);

            watch.Stop();
            Debug.WriteLine($"Processtime in Code: {watch.ElapsedMilliseconds}ms");

            return result;
        }
    }
}