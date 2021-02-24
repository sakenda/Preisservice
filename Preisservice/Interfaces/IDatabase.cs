namespace Preisservice
{
    public interface IDatabase
    {
        UserPriceModel GetProcessedProductPrices(int productID, int userID);
        UserPriceProxy GetRawProductPrices(int productID, int userID);
    }
}