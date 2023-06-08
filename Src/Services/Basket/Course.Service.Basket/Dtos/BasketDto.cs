using Microsoft.AspNetCore.Components.Forms;

namespace Course.Service.Basket.Dtos
{
    public class BasketDto
    {
        public BasketDto()
        {
            basketItems = new List<BasketItemDto>();
        }
        public string UserId { get; set; }
        public string DiscountCode { get; set; }

        public int? DiscountRate { get; set; }
        public List<BasketItemDto> basketItems { get; set; }
        public decimal TotalPriace

        {
            get => basketItems.Sum(x => x.Priace * x.Quantity);

        }

    }
}
