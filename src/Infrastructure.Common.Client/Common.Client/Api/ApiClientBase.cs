using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ProData.Infrastructure.Common.Client.Configuration;
using ProData.Infrastructure.Common.Client.Exceptions;
using ProData.Infrastructure.Common.Client.Extensions;
using ProData.Infrastructure.Common.Exceptions;

namespace ProData.Infrastructure.Common.Client.Api
{
	public abstract class ApiClientBase
	{
		private readonly HttpClient _httpClient;

		protected ApiClientBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

		protected HttpClient HttpClient => _httpClient;

		public PingResult Ping()
		{
			HttpResponseMessage resp = _httpClient.GetAsync("ping").Result;
			resp.EnsureSuccessStatusCode();

			return resp.Content.ReadAsAsync<PingResult>().Result;
		}

        protected async Task<T> GetAsync<T>(string requestUri)
		{
            var httpResponseMessage = await _httpClient.GetAsync(requestUri);

			if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden || httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
				throw new PdException((int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode.ToString());

			httpResponseMessage.EnsureSuccessStatusCode();

			ApiResponse<T> apiResponse = httpResponseMessage.Content.ReadAsAsync<ApiResponse<T>>().Result;
			if (apiResponse.StatusCode != (int)HttpStatusCode.OK)
				throw new PdException(apiResponse.StatusCode, apiResponse.Message);

			return apiResponse.Data;
		}

		protected async Task GetAsync(string requestUri)
		{
            var httpResponseMessage = await _httpClient.GetAsync(requestUri);

			if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden || httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
				throw new PdException((int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode.ToString());

			httpResponseMessage.EnsureSuccessStatusCode();
		}

		protected async Task<TOutput> PostAsync<TOutput>(object content, string requestUri)
        {
            var stringContent = new StringContent(JsonSerializer.Serialize(content, SerializationConfig.Options), Encoding.UTF8, "application/json");

			var httpResponseMessage = await _httpClient.PostAsync(requestUri, stringContent);

			if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden || httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
				throw new PdException((int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode.ToString());

			httpResponseMessage.EnsureSuccessStatusCode();

			var serviceResponse = httpResponseMessage.Content.ReadAsAsync<ApiResponse<TOutput>>().Result;
			if (serviceResponse.StatusCode != (int)HttpStatusCode.OK)
				throw new PdException(serviceResponse.StatusCode, serviceResponse.Message);

			return serviceResponse.Data;
		}
	}
}