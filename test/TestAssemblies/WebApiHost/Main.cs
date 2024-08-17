using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System;
using RougamoDefLib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace WebApiHost
{
    public class Main
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal static ServiceHolder _ServiceHolder;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Main(ServiceHolder serviceHolder)
        {
            _ServiceHolder = serviceHolder;
        }

        public IAsyncDisposable Execute(ServiceHolder serviceHolder) => Execute(serviceHolder, true, true, false);

        public IAsyncDisposable ExecuteNestableScope(ServiceHolder serviceHolder) => Execute(serviceHolder, true, true, true);

        public IAsyncDisposable ExecuteWithoutRougamo(ServiceHolder serviceHolder) => Execute(serviceHolder, false, true, true);

        public IAsyncDisposable ExecuteTransient(ServiceHolder serviceHolder) => Execute(serviceHolder, true, false, true);

        private IAsyncDisposable Execute(ServiceHolder serviceHolder, bool enableRougamo, bool scoped, bool nestableScope)
        {
            IHost host;
#if NET6_0_OR_GREATER
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(BuildConfiguration(enableRougamo, scoped, nestableScope)).Build();
            var startup = new Startup(configuration);
            var builder = WebApplication.CreateBuilder();

            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            startup.Configure(app);

            host = app;
#else
            host = Host
                    .CreateDefaultBuilder()
                    .ConfigureAppConfiguration(builder => builder.AddInMemoryCollection(BuildConfiguration(enableRougamo, scoped, nestableScope)))
                    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                    .Build();
#endif

            host.Start();

            return new HostHolder(host);
        }

        private Dictionary<string, string?> BuildConfiguration(bool enableRougamo, bool scoped, bool nestableScope)
        {
            return new Dictionary<string, string?>()
            {
                { nameof(enableRougamo), enableRougamo.ToString() },
                { nameof(scoped), scoped.ToString()},
                { nameof(nestableScope), nestableScope.ToString() }
            };
        }

        public sealed class HostHolder(IHost host) : IAsyncDisposable
        {
            public async Task WaitForShutdownAsync()
            {
                await host.WaitForShutdownAsync();
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
