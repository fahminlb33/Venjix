using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Venjix.Infrastructure.Database;

namespace Venjix.Infrastructure.Registrations
{
    public static class HealthChecksRegistration
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, string connectionString)
        {
            services.AddHealthChecks()
                .AddCheck("backend", () => HealthCheckResult.Healthy())
                .AddDbContextCheck<VenjixContext>("data-context");

            services.AddHealthChecksUI(settings => { settings.AddHealthCheckEndpoint("main", "/health"); })
                .AddSqliteStorage(connectionString);

            return services;
        }

        public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI(options =>
            {
                options.ApiPath = "/health-api";
                options.UIPath = "/health-ui";
            });

            return app;
        }

        public static void MapCustomHealthChecks(this IEndpointRouteBuilder route)
        {
            route.MapHealthChecks("/health");
            route.MapHealthChecksUI();
        }
    }
}
