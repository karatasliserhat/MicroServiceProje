using StackExchange.Redis;

namespace Course.Service.Basket.Services
{
    public class RedisService
    {
        private ConnectionMultiplexer _connectionMultiplexer;

        private readonly string _Host;
        private readonly int _Port;

        public RedisService(string host, int port)
        {
            _Host = host;
            _Port = port;
        }

        public void Connect() => _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_Host}:{_Port}");
        public IDatabase GetDb(int db = 1) => _connectionMultiplexer.GetDatabase(db);
    }
}
