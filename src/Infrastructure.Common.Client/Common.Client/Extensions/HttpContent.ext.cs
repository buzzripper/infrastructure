using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ProData.Infrastructure.Common.Client.Configuration;

namespace ProData.Infrastructure.Common.Client.Extensions
{
	public static class HttpResponseMessageExtensions
	{
		public static async Task<T> ReadAsAsync<T>(this HttpContent content) where T : class
		{
			var jsonStr = await content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<T>(jsonStr, SerializationConfig.Options);
		}
	}
}
