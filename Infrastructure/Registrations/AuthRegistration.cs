using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Venjix.Infrastructure.Registrations
{
    public static class AuthRegistration
    {
        public static void AddCustomAuth(this IServiceCollection services)
        {
            services.AddAuthentication(x =>
                {
                    x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/login/index";
                    options.LogoutPath = "/login/logout";
                    options.AccessDeniedPath = "/error/unauthorizedpage";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                });
            services.AddAuthorization(authOptions =>
            {
                authOptions.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });
        }

        public static void UseCustomAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
