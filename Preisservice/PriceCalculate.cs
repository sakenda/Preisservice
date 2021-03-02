using System.Threading.Tasks;

namespace PreisService
{
    public class PriceCalculate
    {
        public UserPriceModel ProcessPrices(UserPriceProxy proxy)
        {
            UserPriceModel result = new UserPriceModel();

            result.ProductID = proxy.ProductID;
            result.UserID = proxy.UserID;
            result.PriceBase = proxy.PriceBase;

            if (proxy.UserProductPrice.HasValue)
            {
                result.PriceBase = proxy.UserProductPrice.Value;
                result.PriceTotal = proxy.UserProductPrice.Value;
                result.UserDiscount = 0;
            }
            else
            {
                result.UserDiscount = CalculateDiscount(proxy);
                proxy.PriceTotal = proxy.PriceBase - (proxy.PriceBase * result.UserDiscount.Value);
            }

            if (proxy.PriceShipping.HasValue)
            {
                result.PriceShipping = proxy.PriceShipping;
                result.PriceTotal += proxy.PriceShipping.Value;
            }

            return result;
        }

        private decimal? CalculateDiscount(UserPriceProxy proxy)
        {
            if (proxy.UserDiscount.HasValue)
            {
                return (decimal)proxy.UserDiscount / (decimal)100.00;
            }
            return 0;
        }
    }
}