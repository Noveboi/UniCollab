﻿namespace Saas.Api.Configuration;

internal static class CorsConfiguration
{
    private const string PolicyName = "SaasProjectCors";

    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:Origin").Get<string>();

        if (allowedOrigins is null)
            throw new InvalidOperationException("Please specify one or more allowed CORS origins in appsettings.json.");

        services.AddCors(options =>
        {
            options.AddPolicy(
                name: PolicyName,
                policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod();
                });
        });
    }

    public static void UseConfiguredCors(this WebApplication app)
    {
        app.UseCors(PolicyName);
    }
}