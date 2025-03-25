﻿using AutoMapper.Configuration.Annotations;
using Npgsql;
using RHTech.Teste.API.Application.AuthServices;
using RHTech.Teste.API.Infra.Persistence.Repositories.Dapper;
using RHTech.Teste.API.Infra.Persistence.Repositories.Dapper.Empresas;
using RHTech.Teste.API.Infra.Persistence.Repositories.Dapper.Users;
using RHTech.Teste.API.Interfaces.Application.Authservices;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Empresas;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Users;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace RHTech.Teste.API.Configurations;
[ExcludeFromCodeCoverage]
public static class DependencyInjectionConfig
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnection>((sp) => new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
