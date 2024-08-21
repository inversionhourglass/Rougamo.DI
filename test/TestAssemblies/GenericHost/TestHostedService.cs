using Microsoft.Extensions.Hosting;
using RougamoDefLib;
using RougamoDefLib.Attributes;
using System.Threading;
using System.Threading.Tasks;

namespace GenericHost
{
    internal class TestHostedService(IScopeProvider scopeProvider, ServiceHolder serviceHolder, Locker locker) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var outerScope = scopeProvider.CreateScope())
            {
                Outer1(serviceHolder);

                using (var innerScope1 = scopeProvider.CreateScope())
                {
                    Inner11(serviceHolder);
                    Inner12(serviceHolder);
                }

                using (var innerScope2 = scopeProvider.CreateScope())
                {
                    Inner21(serviceHolder);
                    Inner22(serviceHolder);
                }

                Outer2(serviceHolder);
            }

            locker.Set();
            return Task.CompletedTask;
        }

        [Outer(0)]
        private void Outer1(ServiceHolder serviceHolder) { }

        [Outer(1)]
        private void Outer2(ServiceHolder serviceHolder) { }

        [Inner1(0)]
        private void Inner11(ServiceHolder serviceHolder) { }

        [Inner1(1)]
        private void Inner12(ServiceHolder serviceHolder) { }

        [Inner2(0)]
        private void Inner21(ServiceHolder serviceHolder) { }

        [Inner2(1)]
        private void Inner22(ServiceHolder serviceHolder) { }
    }
}
