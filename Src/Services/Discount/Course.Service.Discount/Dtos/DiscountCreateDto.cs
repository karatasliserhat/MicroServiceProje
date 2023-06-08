namespace Course.Service.Discount.Dtos
{
    public class DiscountCreateDto:BaseDto
    {
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
