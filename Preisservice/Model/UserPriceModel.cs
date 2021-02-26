namespace Preisservice
{
    public class UserPriceModel
    {
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public decimal PriceBase { get; set; }
        public decimal? PriceShipping { get; set; }
        public decimal? UserDiscount { get; set; }
        public decimal PriceTotal { get; set; }

        public UserPriceModel()
        {
        }

        public UserPriceModel(
            int userID,
            int productID,
            decimal priceBase,
            decimal? priceShipping,
            decimal? userDiscount,
            decimal priceTotal)
        {
            UserID = userID;
            ProductID = productID;
            PriceBase = priceBase;
            PriceShipping = priceShipping;
            UserDiscount = userDiscount;
            PriceTotal = priceTotal;
        }

        //public UserPriceModel(UserPriceProxy proxy)
        //{
        //    UserID = proxy.UserID;
        //    ProductID = proxy.ProductID;
        //    PriceBase = proxy.PriceBase;
        //    PriceShipping = proxy.PriceShipping;
        //    UserDiscount = proxy.UserDiscount;
        //    PriceTotal = proxy.PriceTotal;
        //}
    }
}