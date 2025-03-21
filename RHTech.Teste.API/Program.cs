using Asp.Versioning.ApiExplorer;
using RHTech.Teste.API.Configurations;
using RHTech.Teste.API.Infra.Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApiConfig()
    .ResolveDependencies(builder.Configuration)
    .AddEntityDataBaseConfig(builder.Configuration)
    .AddAuthConfig(builder.Configuration)
    .AddHttpClientConfig(builder.Configuration);

var app = builder.Build();
IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();
app.UseApiConfig(app.Environment, descriptions);
app.Run();
