using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Venjix.Infrastructure.Registrations
{
    public static class NewtonsoftRegistration
    {
        public static IMvcBuilder AddCustomNewtonsoftJson(this IMvcBuilder builder)
        {
            builder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            return builder;
        }
    }
}
