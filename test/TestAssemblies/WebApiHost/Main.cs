using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System;
using RougamoDefLib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Linq;

namespace WebApiHost
{
    public class Main
    {
        public HostHolder Execute(ServiceHolder serviceHolder) => Execute(serviceHolder, true, true, false);

        public HostHolder ExecuteNestableScope(ServiceHolder serviceHolder) => Execute(serviceHolder, true, true, true);

        public HostHolder ExecuteWithoutRougamo(ServiceHolder serviceHolder) => Execute(serviceHolder, false, true, true);

        public HostHolder ExecuteTransient(ServiceHolder serviceHolder) => Execute(serviceHolder, true, false, true);

        private HostHolder Execute(ServiceHolder serviceHolder, bool enableRougamo, bool scoped, bool nestableScope)
        {
            ServiceProviderHolderAccessor.SetRootNull();

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

                services.AddSingleton(serviceHolder);
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

        public sealed class HostHolder(IHost host, string address) : IAsyncDisposable
        {
            public string Address => address;

            public async Task WaitForShutdownAsync()
            {
                await host.WaitForShutdownAsync();
            }

            public Task StopAsync()
            {
                return host.StopAsync();
            }

            public async ValueTask DisposeAsync()
            {
                if (host is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else
                {
                    host.Dispose();
                }
            }
        }
    }
}
