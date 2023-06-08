namespace Course.Service.Discount.Models
{

    [Dapper.Contrib.Extensions.Table("discount")]
    public class DiscountEntity:BaseEntity
    {
       
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
    }
}
