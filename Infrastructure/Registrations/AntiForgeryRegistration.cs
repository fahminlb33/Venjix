using Microsoft.Extensions.DependencyInjection;

namespace Venjix.Infrastructure.Registrations
{
    public static class AntiForgeryRegistration
    {
        public static void AddCustomAntiForgery(this IServiceCollection services)
        {
            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "VAntiForgery";
                options.HeaderName = "X-CSRF-TOKEN";
                options.SuppressXFrameOptionsHeader = false;
            });
        }
    }
}
