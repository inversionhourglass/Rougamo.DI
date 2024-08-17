using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RougamoDefLib;
using System;
using System.Threading.Tasks;

namespace GenericHost
{
    public class Main
    {
        public IAsyncDisposable Execute(ServiceHolder serviceHolder) => Execute(serviceHolder, true, true);

        public IAsyncDisposable ExecuteWithoutRougamo(ServiceHolder serviceHolder) => Execute(serviceHolder, false, true);

        public IAsyncDisposable ExecuteTransient(ServiceHolder serviceHolder) => Execute(serviceHolder, true, false);

        private IAsyncDisposable Execute(ServiceHolder serviceHolder, bool enableRougamo, bool scoped)
        {
            var builder = Host.CreateDefaultBuilder();

            builder.ConfigureServices(services =>
            {
                var descriptor = new ServiceDescriptor(typeof(ITestService), typeof(TestService), scoped ? ServiceLifetime.Scoped : ServiceLifetime.Transient);
                
                services.AddSingleton(serviceHolder);
                services.Add(descriptor);
                services.AddHostedService<TestHostedService>();

                if (enableRougamo)
                {
                    services.AddRougamoGenericHost();
                }
                else
                {
                    services.AddServiceScopeAccessor();
                }
            });

            var host = builder.Build();

            host.Start();

            return new HostHolder(host);
        }

        public sealed class HostHolder(IHost host) : IAsyncDisposable
        {
            public async ValueTask DisposeAsync()
            {
                await host.StopAsync();
            }
        }
    }
}
