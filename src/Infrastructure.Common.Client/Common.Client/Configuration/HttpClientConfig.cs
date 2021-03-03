
namespace ProData.Infrastructure.Common.Client.Configuration
{
    public class HttpClientConfig
    {
        public string BaseAddress { get; set; }
        public int RetryCount { get; set; }
        public int HandlerLifetimeMins { get; set; }
    }
}
