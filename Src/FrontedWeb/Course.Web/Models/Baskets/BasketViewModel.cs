using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Course.Web.Models.Baskets
{
    public class BasketViewModel
    {
        public BasketViewModel()
        {
            _basketItems = new List<BasketItemViewModel>();
        }
        public string UserId { get; set; }
        public string DiscountCode { get; set; }

        private List<BasketItemViewModel> _basketItems;

        public int? DiscountRate { get; set; }
        public List<BasketItemViewModel> BasketItems
        {
            get
            {

                if (HasDiscount)
                {

                    _basketItems.ForEach(x =>
                    {
                        var discountPrice = x.Priace * ((Decimal)DiscountRate.Value / 100);
                        x.AppliedDiscount(Math.Round(x.Priace - discountPrice, 2));
                    });
                }
                return _basketItems;
            }
            set
            {
                _basketItems = value;
            }
        }
        public decimal TotalPriace

        {
            get => _basketItems.Sum(x => x.GetCurrentPrice);

        }

        public bool HasDiscount
        {
            get => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;
        }

        public void ApplyCode(string code, int rate)
        {
            DiscountCode = code;
            DiscountRate = rate;
        }
        public void CancelAppDiscount()
        {
            DiscountCode = null;
            DiscountRate = null;
        }
    }
}
