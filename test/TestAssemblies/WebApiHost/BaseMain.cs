using Microsoft.Extensions.Hosting;
using RougamoDefLib;
using System.Threading.Tasks;
using System;

namespace WebApiHost
{
    public abstract class BaseMain
    {
        public HostHolder Execute(ServiceHolder serviceHolder) => Execute(serviceHolder, true, true, false);

        public HostHolder ExecuteDisableNestableScope(ServiceHolder serviceHolder) => Execute(serviceHolder, true, true, true);

        public HostHolder ExecuteWithoutRougamo(ServiceHolder serviceHolder) => Execute(serviceHolder, false, true, false);

        public HostHolder ExecuteTransient(ServiceHolder serviceHolder) => Execute(serviceHolder, true, false, false);

        protected abstract HostHolder Execute(ServiceHolder serviceHolder, bool enableRougamo, bool scoped, bool disableNestableScope);

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
