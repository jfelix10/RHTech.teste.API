using Polly;
using System.Diagnostics.CodeAnalysis;

namespace RHTech.Teste.API.Configurations;

[ExcludeFromCodeCoverage]
public static class HttpClientConfig
{
    public static IServiceCollection AddHttpClientConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("AIApi", client =>
        {
            client.BaseAddress = new Uri(configuration["InternalCall:AIApi:Uri"]!);
        })
        .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(
        [
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(10)
        ]));

        return services;
    }
}
