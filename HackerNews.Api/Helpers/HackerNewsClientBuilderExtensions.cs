using HackerNews.Api.ApiClients;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using Refit;

namespace HackerNews.Api.Helpers;

public static class HackerNewsClientBuilderExtensions
{
    public static IServiceCollection AddHackerNewsApiClient(
        this IServiceCollection services,
        string baseUrl,
        int retryCount = 0)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(baseUrl);

        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt - 1)));

        var sertializationSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
        };

        services.AddRefitClient<IHackerNewsApiClient>(new RefitSettings(new NewtonsoftJsonContentSerializer(sertializationSettings)))
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl!))
            .AddPolicyHandler(retryPolicy);

        return services;
    }
}
