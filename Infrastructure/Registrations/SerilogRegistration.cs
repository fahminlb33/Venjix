using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Venjix.Infrastructure.Registrations
{
    public static class SerilogRegistration
    {
        public static IApplicationBuilder UseCustomSerilog(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = EnrichFromRequest;
                options.GetLevel = GetCustomLevel;
            });

            return app;
        }

        private static LogEventLevel GetCustomLevel(HttpContext context, double _, Exception ex)
        {
            if (ex != null)
            {
                return LogEventLevel.Error;
            }

            if (context.Response.StatusCode > 499)
            {
                return LogEventLevel.Error;
            }

            return IsHealthCheckEndpoint(context)
                ? LogEventLevel.Verbose
                : LogEventLevel.Information;
        }

        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            var request = httpContext.Request;

            diagnosticContext.Set("Host", request.Host);
            diagnosticContext.Set("Protocol", request.Protocol);
            diagnosticContext.Set("Scheme", request.Scheme);

            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

            var endpoint = httpContext.GetEndpoint();
            if (endpoint != null)
            {
                diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            }
        }

        private static bool IsHealthCheckEndpoint(HttpContext ctx)
        {
            var endpoint = ctx.GetEndpoint();
            return endpoint != null && string.Equals(endpoint.DisplayName, "Health checks", StringComparison.Ordinal);
        }
    }
}
