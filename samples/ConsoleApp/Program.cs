using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLib;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder();

            builder.UsePinnedScopeServiceProvider();

            builder.ConfigureServices(servicecs =>
            {
                servicecs.AddHostedService<RunningService>();
                servicecs.AddScoped<TestService>();
            });

            var host = builder.Build();

            host.Run();
        }
    }
}
