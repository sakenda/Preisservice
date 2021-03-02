using PreisService.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PreisService
{
    public interface IDatabase
    {
        Task<UserPriceModel> GetProcessedProductPricesAsync(int productID, int userID);
        Task<UserPriceProxy> GetRawProductPricesAsync(int productID, int userID);
        List<ProductModel> GetAllProducts();
    }
}