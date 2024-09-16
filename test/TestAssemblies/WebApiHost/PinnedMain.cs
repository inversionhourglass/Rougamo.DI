using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rougamo.Context;
using RougamoDefLib;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace WebApiHost
{
    public class PinnedMain : BaseMain
    {
        protected override HostHolder Execute(ServiceHolder serviceHolder, bool enableRougamo, bool scoped, bool disableNestableScope)
        {
            PinnedScopeAccessor.SetRootNull();
            ContextExtensions.SetPinned();

            IHost host;
#if NET6_0_OR_GREATER
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls("http://127.0.0.1:0");

            if (enableRougamo)
            {
                builder.Host.UsePinnedScopeServiceProvider();
            }

            ConfigureServices(builder.Services);

            var app = builder.Build();

            Configure(app);

            host = app;
#else
            host = Host
                    .CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(builder =>
                    {
                        builder
                            .ConfigureServices(ConfigureServices)
                            .Configure(Configure)
                            .UseUrls("http://127.0.0.1:0");
                    })
                    .If(enableRougamo, builder => builder.UsePinnedScopeServiceProvider())
                    .Build();
#endif

            host.Start();

            var addressesFeature = host.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>();

            return new HostHolder(host, addressesFeature!.Addresses.First());

            void ConfigureServices(IServiceCollection services)
            {
                services.AddControllers().AddCurrentApplicationPart();

                var descriptor = new ServiceDescriptor(typeof(ITestService), typeof(TestService), scoped ? ServiceLifetime.Scoped : ServiceLifetime.Transient);

                services.AddTransient<IScopeProvider, PinnedScopeProvider>();
                services.AddHttpContextAccessor();
                services.AddSingleton(serviceHolder);
                services.Add(descriptor);
            }

            void Configure(IApplicationBuilder app)
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
}
