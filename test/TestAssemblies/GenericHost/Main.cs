using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rougamo.Context;
using RougamoDefLib;

namespace GenericHost
{
    public class Main : BaseMain
    {
        protected override HostHolder Execute(ServiceHolder serviceHolder, bool enableRougamo, bool scoped)
        {
            ServiceProviderHolderAccessor.SetRootNull();
            ContextExtensions.SetMicrosoft();

            var locker = new Locker();
            var builder = Host.CreateDefaultBuilder();

            builder.ConfigureServices(services =>
            {
                var descriptor = new ServiceDescriptor(typeof(ITestService), typeof(TestService), scoped ? ServiceLifetime.Scoped : ServiceLifetime.Transient);

                services.AddSingleton(locker);
                services.AddSingleton(serviceHolder);
                services.Add(descriptor);
                services.AddTransient<IScopeProvider, MicrosoftScopeProvider>();
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
    }
}
