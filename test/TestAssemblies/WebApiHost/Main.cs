using Microsoft.Extensions.Hosting;
using RougamoDefLib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Linq;
using Rougamo.Context;

namespace WebApiHost
{
    public class Main : BaseMain
    {
        protected override HostHolder Execute(ServiceHolder serviceHolder, bool enableRougamo, bool scoped, bool disableNestableScope)
        {
            ServiceProviderHolderAccessor.SetRootNull();
            ContextExtensions.SetMicrosoft();

            IHost host;
#if NET6_0_OR_GREATER
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls("http://127.0.0.1:0");

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
                    .Build();
#endif

            host.Start();

            var addressesFeature = host.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>();

            return new HostHolder(host, addressesFeature!.Addresses.First());

            void ConfigureServices(IServiceCollection services)
            {
                services.AddControllers().AddCurrentApplicationPart();

                var descriptor = new ServiceDescriptor(typeof(ITestService), typeof(TestService), scoped ? ServiceLifetime.Scoped : ServiceLifetime.Transient);

                services.AddTransient<IScopeProvider, MicrosoftScopeProvider>();
                services.AddSingleton(serviceHolder);
                services.Add(descriptor);

                if (disableNestableScope)
                {
                    services.AddHttpContextScopeAccessor();
                }
                if (enableRougamo)
                {
                    services.AddRougamoAspNetCore();
                }
                else
                {
                    services.AddNestableHttpContextScopeAccessor();
                }
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
