using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RougamoDefLib;

namespace WebApiHost
{
    public class Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var enableRougamo = configuration.GetValue<bool>("enableRougamo");
            var scoped = configuration.GetValue<bool>("scoped");
            var nestableScope = configuration.GetValue<bool>("nestableScope");

            var descriptor = new ServiceDescriptor(typeof(ITestService), typeof(TestService), scoped ? ServiceLifetime.Scoped : ServiceLifetime.Transient);

            services.AddSingleton(Main._ServiceHolder);
            services.Add(descriptor);

            if (nestableScope)
            {
                services.AddNestableHttpContextScopeAccessor();
            }
            if (enableRougamo)
            {
                services.AddRougamoAspNetCore();
            }
            else
            {
                services.AddHttpContextScopeAccessor();
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
