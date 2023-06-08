namespace Course.Services.FreePayment.Dtos
{
    public class PaymentDto
    {
      
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }

        public OrderDto Order { get; set; }
    }
}
