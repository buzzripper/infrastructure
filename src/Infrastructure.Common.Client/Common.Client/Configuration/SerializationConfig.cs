using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProData.Infrastructure.Common.Client.Configuration
{
	public static class SerializationConfig
	{
		static SerializationConfig()
		{
            Options = new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true
			};
            Options.Converters.Add(new JsonStringEnumConverter());
		}

		public static JsonSerializerOptions Options { get; }
	}
}
