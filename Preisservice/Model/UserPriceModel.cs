namespace PreisService
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
    }
}