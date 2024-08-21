using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rougamo.Context;
using RougamoDefLib;

namespace GenericHost
{
    public class AutofacMain : BaseMain
    {
        protected override HostHolder Execute(ServiceHolder serviceHolder, bool enableRougamo, bool scoped)
        {
            ContainerHolderAccessor.SetRootNull();
            ContextExtensions.SetAutofac();

            var locker = new Locker();
            var builder = Host.CreateDefaultBuilder();

            builder
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    if (scoped)
                    {
                        builder.RegisterType<TestService>().As<ITestService>().InstancePerLifetimeScope();
                    }
                    else
                    {
                        builder.RegisterType<TestService>().As<ITestService>().InstancePerDependency();
                    }
                    builder.RegisterInstance(locker).SingleInstance();
                    builder.RegisterInstance(serviceHolder).SingleInstance();
                    if (enableRougamo)
                    {
                        builder.RegisterRougamo();
                    }
                    else
                    {
                        builder.RegisterILifetimeScopeAccessor();
                    }
                    builder.RegisterType<AutofacScopeProvider>().As<IScopeProvider>().InstancePerDependency();
                })
                .ConfigureServices(services => services.AddHostedService<TestHostedService>());

            var host = builder.Build();

            host.Start();

            return new HostHolder(host, locker);
        }
    }
}
