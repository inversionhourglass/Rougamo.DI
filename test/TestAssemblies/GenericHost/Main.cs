using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RougamoDefLib;
using System;
using System.Threading.Tasks;

namespace GenericHost
{
    public class Main
    {
        public HostHolder Execute(ServiceHolder serviceHolder) => Execute(serviceHolder, true, true);

        public HostHolder ExecuteWithoutRougamo(ServiceHolder serviceHolder) => Execute(serviceHolder, false, true);

        public HostHolder ExecuteTransient(ServiceHolder serviceHolder) => Execute(serviceHolder, true, false);

        private HostHolder Execute(ServiceHolder serviceHolder, bool enableRougamo, bool scoped)
        {
            ServiceProviderHolderAccessor.SetRootNull();

            var locker = new Locker();
            var builder = Host.CreateDefaultBuilder();

            builder.ConfigureServices(services =>
            {
                var descriptor = new ServiceDescriptor(typeof(ITestService), typeof(TestService), scoped ? ServiceLifetime.Scoped : ServiceLifetime.Transient);

                services.AddSingleton(locker);
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

            return new HostHolder(host, locker);
        }

        public sealed class HostHolder(IHost host, Locker locker) : IAsyncDisposable
        {
            public Task WaitForExecuteAsync()
            {
                return locker.WaitForExecuteAsync();
            }

            public async ValueTask DisposeAsync()
            {
                await host.StopAsync();

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
