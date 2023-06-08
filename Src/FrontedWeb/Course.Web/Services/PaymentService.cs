using Course.Web.Models.FakePayments;
using Course.Web.Services.Interfaces;

namespace Course.Web.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ReceivedPayment(PaymentInfoInput paymentInfoInput)
        {
            var response = await _httpClient.PostAsJsonAsync<PaymentInfoInput>("freepayments", paymentInfoInput);

            return response.IsSuccessStatusCode;
        }
    }
}
