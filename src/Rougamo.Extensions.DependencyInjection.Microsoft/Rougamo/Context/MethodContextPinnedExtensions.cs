using DependencyInjection.StaticAccessor;
using Microsoft.Extensions.DependencyInjection;
using Rougamo.Extensions.DependencyInjection.Microsoft;
using System;
using System.Collections.Generic;

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
            var rootServices = GetRootServiceProvider(context);
            if (rootServices != null)
            {
                var forwards = rootServices.GetServices<IMethodBaseScopeForward>();
                foreach (var forward in forwards)
                {
                    var scope = forward.GetScope(context.Target, context.Method);
                    if (scope != null) return scope.ServiceProvider;
                }
            }

            var scopedServices = PinnedScope.Scope?.ServiceProvider;
            if (scopedServices != null) return scopedServices;

            if (rootServices != null)
            {
                var goalies = rootServices.GetServices<IMethodBaseScopeGoalie>();
                foreach (var goalie in goalies)
                {
                    var scope = goalie.GetScope(context.Target, context.Method);
                    if (scope != null) return scope.ServiceProvider;
                }
            }

            return PinnedScope.RootServices;
        }

        /// <summary>
        /// </summary>
        public static object? GetService(this MethodContext context, Type serviceType)
        {
            return context.GetServiceProvider()?.GetService(serviceType);
        }

        /// <summary>
        /// </summary>
        public static object GetRequiredService(this MethodContext context, Type serviceType)
        {
            var scopedServices = context.GetServiceProvider();
            if (scopedServices == null) throw new InvalidOperationException("Cannot get the IServiceProvider instance.");

            return scopedServices.GetRequiredService(serviceType);
        }

        /// <summary>
        /// </summary>
        public static IEnumerable<object> GetServices(this MethodContext context, Type serviceType)
        {
            var scopedServices = context.GetServiceProvider();
            return scopedServices == null ? [] : scopedServices.GetServices(serviceType);
        }

        /// <summary>
        /// </summary>
        public static T? GetService<T>(this MethodContext context)
        {
            var scopedServices = context.GetServiceProvider();
            return scopedServices == null ? default : scopedServices.GetService<T>();
        }

        /// <summary>
        /// </summary>
        public static T GetRequiredService<T>(this MethodContext context)
        {
            var scopedServices = context.GetServiceProvider();
            if (scopedServices == null) throw new InvalidOperationException("Cannot get the IServiceProvider instance.");

            return scopedServices.GetRequiredService<T>();
        }

        /// <summary>
        /// </summary>
        public static IEnumerable<T> GetServices<T>(this MethodContext context)
        {
            var scopedServices = context.GetServiceProvider();
            return scopedServices == null ? [] : scopedServices.GetServices<T>();
        }
    }
}
