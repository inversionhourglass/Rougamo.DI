using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Rougamo.Extensions.DependencyInjection
{
    internal class SetProviderHostedService : BackgroundService
    {
        public SetProviderHostedService(IServiceProvider provider)
        {
            ServiceProviderHolder.Root = provider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
