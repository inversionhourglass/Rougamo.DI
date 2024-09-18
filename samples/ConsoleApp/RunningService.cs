using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLib;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class RunningService(IServiceProvider serviceProvider) : IHostedService
    {
        [Getter]
        public static TestService GetTestService() => throw new NotImplementedException();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scopeProvider = serviceProvider.CreateScope())
            {
                var isEqual = scopeProvider.ServiceProvider.GetRequiredService<TestService>() == GetTestService();

                Console.WriteLine("----------------------");
                Console.WriteLine($"IsEqual: {isEqual}");
                Console.WriteLine("----------------------");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
