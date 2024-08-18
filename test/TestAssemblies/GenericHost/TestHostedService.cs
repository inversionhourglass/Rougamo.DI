using Microsoft.Extensions.Hosting;
using RougamoDefLib;
using RougamoDefLib.Attributes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GenericHost
{
    internal class TestHostedService(IServiceProvider provider, ServiceHolder serviceHolder, Locker locker) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var outerScope = provider.CreateResolvableScope())
            {
                Outer1(serviceHolder);

                using (var innerScope1 = provider.CreateResolvableScope())
                {
                    Inner11(serviceHolder);
                    Inner12(serviceHolder);
                }

                using (var innerScope2 = provider.CreateResolvableScope())
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
