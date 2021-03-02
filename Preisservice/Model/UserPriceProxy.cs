namespace PreisService
{
    public class UserPriceProxy
    {
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public decimal PriceBase { get; set; }
        public decimal? PriceShipping { get; set; }
        public decimal? UserDiscount { get; set; }
        public decimal? UserProductPrice { get; set; }
        public decimal PriceTotal { get; set; }

        public UserPriceProxy(
            int userID,
            int productID,
            decimal priceBase,
            decimal? priceShipping,
            decimal? userDiscount,
            decimal? userProductPrice)
        {
            UserID = userID;
            ProductID = productID;
            PriceBase = priceBase;
            PriceShipping = priceShipping;
            UserDiscount = userDiscount;
            UserProductPrice = userProductPrice;
        }
    }
}