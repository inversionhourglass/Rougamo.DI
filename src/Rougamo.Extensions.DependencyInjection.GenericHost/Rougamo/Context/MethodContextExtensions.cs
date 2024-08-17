using Microsoft.Extensions.DependencyInjection;
using Rougamo.Extensions.DependencyInjection;
using System;

namespace Rougamo.Context
{
    /// <summary>
    /// </summary>
    public static class MethodContextExtensions
    {
        /// <summary>
        /// Get the root <see cref="IServiceProvider"/>
        /// </summary>
        public static IServiceProvider? GetRootServiceProvider(this MethodContext context)
        {
            return ServiceProviderHolder.Root;
        }

        /// <summary>
        /// Get the current scope <see cref="IServiceProvider"/>, return root <see cref="IServiceProvider"/> if it is not within a scope.
        /// </summary>
        public static IServiceProvider? GetServiceProvider(this MethodContext context)
        {
            var provider = ServiceProviderHolder.Root;
            if (provider == null) return null;

            return provider.GetService<ISmartServiceProvider>() ?? provider;
        }
    }
}
