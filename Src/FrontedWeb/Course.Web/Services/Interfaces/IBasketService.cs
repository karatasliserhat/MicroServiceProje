using Course.Web.Models.Baskets;

namespace Course.Web.Services.Interfaces
{
    public interface IBasketService
    {
        Task<BasketViewModel> GetBasket();
        Task<bool> SaveOrUpdate(BasketViewModel basketViewModel);
        Task<bool> Delete();
        Task AddBasketItem(BasketItemViewModel basketItemViewModel);
        Task<bool> DeleteBasketItem(string courseId);

        Task<bool> ApplyDiscount(string discountCode);
        Task<bool> CancelApplyDiscount();
    }
}
