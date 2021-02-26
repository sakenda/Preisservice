using System.Threading.Tasks;

namespace Preisservice
{
    public interface IDatabase
    {
        Task<UserPriceModel> GetProcessedProductPricesAsync(int productID, int userID);
        Task<UserPriceProxy> GetRawProductPricesAsync(int productID, int userID);
    }
}