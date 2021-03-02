using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreisService.Model
{
    public class ProductModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Supplier { get; set; }
        public string Category { get; set; }
    }
}