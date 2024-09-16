using DependencyInjection.StaticAccessor;
using System;

namespace Rougamo.Context
{
    /// <summary>
    /// </summary>
    public static class MethodContextPinnedExtensions
    {
        /// <summary>
        /// Get the root <see cref="IServiceProvider"/>
        /// </summary>
        public static IServiceProvider? GetRootServiceProvider(this MethodContext context)
        {
            return PinnedScope.RootServices;
        }

        /// <summary>
        /// Get the current scope <see cref="IServiceProvider"/>, return root <see cref="IServiceProvider"/> if it is not within a scope.
        /// </summary>
        public static IServiceProvider? GetServiceProvider(this MethodContext context)
        {
            return PinnedScope.ScopedServices;
        }
    }
}
