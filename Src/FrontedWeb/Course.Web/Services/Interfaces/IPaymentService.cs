using Course.Web.Models.FakePayments;
using Microsoft.Build.Framework;

namespace Course.Web.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ReceivedPayment(PaymentInfoInput paymentInfoInput);
    }
}
