namespace Preisservice
{
    public class PriceCalculate
    {
        public UserPriceModel ProcessPrices(UserPriceProxy proxy)
        {
            decimal discount = CalculateDiscount(proxy);

            proxy.PriceBase = SetBasePrice(proxy);
            proxy.PriceTotal = proxy.PriceBase - (proxy.PriceBase * (decimal)discount);

            if (proxy.PriceShipping != null)
                proxy.PriceTotal += (decimal)proxy.PriceShipping;

            return new UserPriceModel(proxy);
        }

        private decimal SetBasePrice(UserPriceProxy proxy)
        {
            if (proxy.UserProductPrice != null)
                return (decimal)proxy.UserProductPrice;
            else
                return proxy.PriceBase;
        }

        private decimal CalculateDiscount(UserPriceProxy proxy)
        {
            if (proxy.UserDiscount != null)
                return (decimal)proxy.UserDiscount / (decimal)100.00;
            return 0;
        }
    }
}