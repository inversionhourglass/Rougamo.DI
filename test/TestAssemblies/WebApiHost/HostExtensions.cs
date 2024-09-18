using Microsoft.Extensions.Hosting;
using System;

namespace WebApiHost
{
    public static class HostExtensions
    {
        public static IHostBuilder If(this IHostBuilder builder, bool condition, Action<IHostBuilder> action)
        {
            if (condition)
            {
                action(builder);
            }

            return builder;
        }
    }
}
