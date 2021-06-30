using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Venjix.Infrastructure.Database;
using Venjix.Infrastructure.Registrations;
using Venjix.Infrastructure.Services.DataTables;
using Venjix.Infrastructure.Services.Forecasting;
using Venjix.Infrastructure.Services.Options;
using Venjix.Infrastructure.Services.Telegram;
using Venjix.Infrastructure.Services.Triggers;

namespace Venjix
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // views
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddCustomNewtonsoftJson();
               
            // routing
            services.AddCustomCors();
            services.AddCustomAuth();
            services.AddCustomAntiForgery();
            services.AddRouting(options => options.LowercaseUrls = true);

            // data access
            services.AddDbContext<VenjixContext>(options => options.UseSqlite(Configuration.GetConnectionString("VenjixContext")));
            
            // health checks
            services.AddCustomHealthChecks(Configuration.GetConnectionString("VenjixHealthContext"));

            // infrastructure
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(Startup));

            // register services
            services.AddTransient<IDataTables, DataTables>();
            services.AddTransient<IForecastingService, ForecastingService>();
            
            services.AddScoped<ITriggerRunnerService, TriggerRunnerService>();

            services.AddSingleton<ITelegramService, TelegramService>();
            services.AddSingleton<IVenjixOptionsService, VenjixOptionsService>(options =>
            {
                var instance = new VenjixOptionsService();
                instance.Reload().GetAwaiter().GetResult();
                return instance;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // exception handler page
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error/index");
            }

            // logging
            app.UseCustomSerilog();

            // routing
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // security
            app.UseCustomAuth();
            app.UseCustomCors();

            // health checks
            app.UseCustomHealthChecks();

            // register route templates
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "APIs",
                    pattern: "api/{controller=ApiData}/{action=SaveDataByQuery}/{id?}");
                endpoints.MapCustomHealthChecks();
            });
        }
    }
}