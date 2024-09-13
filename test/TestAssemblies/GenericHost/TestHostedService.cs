using Microsoft.Extensions.Hosting;
using RougamoDefLib;
using RougamoDefLib.Attributes;
using System.Threading;
using System.Threading.Tasks;

namespace GenericHost
{
    internal class TestHostedService(IScopeProvider scopeProvider, ServiceHolder serviceHolder, Locker locker) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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

                var locker1 = new EventWaitHandle(false, EventResetMode.ManualReset);
                var locker2 = new EventWaitHandle(false, EventResetMode.ManualReset);
                var t1 = Task.Run(() =>
                {
                    locker1.WaitOne();
                    Parallel1(serviceHolder);
                    locker2.Set();
                });
                var t2 = Task.Run(() =>
                {
                    using (var pScope = scopeProvider.CreateScope())
                    {
                        Parallel2(serviceHolder);
                        locker1.Set();
                        locker2.WaitOne();
                    }
                });
                await Task.WhenAll(t1, t2);

                Outer2(serviceHolder);
            }

            locker.Set();
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

        [Parallel(0)]
        private void Parallel1(ServiceHolder serviceHolder) { }

        [Parallel(1)]
        private void Parallel2(ServiceHolder serviceHolder) { }
    }
}
