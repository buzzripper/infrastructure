using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using ProData.Infrastructure.Common.Client.Configuration;

namespace ProData.Infrastructure.Common.Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection InitializeCommonClient(this IServiceCollection collection)
        {
            return collection;
        }
    }
}
