namespace Course.Web.Settings
{
    public class ClientSettings
    {
        public Client WebClientApp { get; set; }
        public Client WebClientAppForUser { get; set; }
    }
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
