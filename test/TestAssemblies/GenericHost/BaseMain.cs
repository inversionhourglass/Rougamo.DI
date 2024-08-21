using Microsoft.Extensions.Hosting;
using RougamoDefLib;
using System;
using System.Threading.Tasks;

namespace GenericHost
{
    public abstract class BaseMain
    {
        public HostHolder Execute(ServiceHolder serviceHolder) => Execute(serviceHolder, true, true);

        public HostHolder ExecuteWithoutRougamo(ServiceHolder serviceHolder) => Execute(serviceHolder, false, true);

        public HostHolder ExecuteTransient(ServiceHolder serviceHolder) => Execute(serviceHolder, true, false);

        protected abstract HostHolder Execute(ServiceHolder serviceHolder, bool enableRougamo, bool scoped);

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
