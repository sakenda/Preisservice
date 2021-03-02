namespace PreisClient
{
    public class RequestModel
    {
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public decimal PriceBase { get; set; }
        public decimal? PriceShipping { get; set; }
        public decimal? UserDiscount { get; set; }
        public decimal PriceTotal { get; set; }
    }
}