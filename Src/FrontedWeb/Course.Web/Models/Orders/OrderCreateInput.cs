namespace Course.Web.Models.Orders
{
    public class OrderCreateInput
    {
        public OrderCreateInput()
        {
            OrderItems = new List<OrderItemCreateInput>();
        }
        public string BuyerId { get; set; }
        public List<OrderItemCreateInput> OrderItems { get; set; }

        public AdressCreateInput Adress { get; set; }
    }
}
