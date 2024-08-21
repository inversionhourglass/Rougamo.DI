using Microsoft.Extensions.Hosting;
using RougamoDefLib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Linq;
using Rougamo.Context;
using Autofac.Extensions.DependencyInjection;
using Autofac;

namespace WebApiHost
{
    public class AutofacMain : BaseMain
    {
        protected override HostHolder Execute(ServiceHolder serviceHolder, bool enableRougamo, bool scoped, bool nestableScope)
        {
            ContainerHolderAccessor.SetRootNull();
            ContextExtensions.SetAutofac();

            IHost host;
#if NET6_0_OR_GREATER
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls("http://127.0.0.1:0");
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(ConfigureContainer);

            ConfigureServices(builder.Services);

            var app = builder.Build();
            
            Configure(app);

            host = app;
#else
            host = Host
                    .CreateDefaultBuilder()
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
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

            void ConfigureContainer(ContainerBuilder builder)
            {
                if (scoped)
                {
                    builder.RegisterType<TestService>().As<ITestService>().InstancePerLifetimeScope();
                }
                else
                {
                    builder.RegisterType<TestService>().As<ITestService>().InstancePerDependency();
                }
                builder.RegisterInstance(serviceHolder).SingleInstance();
                builder.RegisterType<AutofacScopeProvider>().As<IScopeProvider>().InstancePerDependency();

                if (nestableScope)
                {
                    builder.RegisterAutofacNestableHttpContextScopeAccessor();
                }
                if (enableRougamo)
                {
                    builder.RegisterRougamoAspNetCore();
                }
                else
                {
                    builder.RegisterAutofacHttpContextScopeAccessor();
                }
            }

            void ConfigureServices(IServiceCollection services)
            {
                services.AddControllers().AddCurrentApplicationPart();
                services.AddHttpContextAccessor();
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
