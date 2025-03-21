using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RHTech.Teste.API.Infra.Persistence.Contexts.EFCore;
using System;
using System.Text;

namespace RHTech.Teste.API.Configurations
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new (1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();

                }).AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "V1 - API", Version = "v1.0" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "V2 - API", Version = "v2.0" });
            });


            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder =>
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly))
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }

        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env, IReadOnlyList<ApiVersionDescription> descriptions)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI V1.0");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebAPI V2.0");
                });
            }
            else
            {
                app.UseCors("Production");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }

        public static IServiceCollection AddEntityDataBaseConfig(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<RHTechContext>(options =>
            //      options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            //services.AddEntityFrameworkNpgsql();
            return services;
        }

        public static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!))
                };
            });

            return services;
        }
    }
}
