using Microsoft.AspNetCore.Mvc;
using PreisService.Model;
using System.Collections.Generic;

namespace PreisService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IDatabase _database;

        public ProductController(IDatabase service)
        {
            _database = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("All")]
        [HttpGet]
        public List<ProductModel> GetAllProducts()
        {
            // https://localhost:44378/product/all
            return _database.GetAllProducts();
        }
    }
}