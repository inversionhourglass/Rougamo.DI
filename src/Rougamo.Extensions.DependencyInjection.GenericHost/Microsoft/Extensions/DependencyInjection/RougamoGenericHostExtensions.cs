using Microsoft.Extensions.DependencyInjection.Extensions;
using Rougamo.Extensions.DependencyInjection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// </summary>
    public static class RougamoGenericHostExtensions
    {
        /// <summary>
        /// Create a scope that can be resolved from the root ServiceProvider using the interface <see cref="IServiceScopeAccessor"/>
        /// </summary>
        public static IServiceScope CreateResolvableScope(this IServiceProvider provider)
        {
            var accessor = provider.GetRequiredService<IServiceScopeAccessor>();
            var scope = provider.CreateScope();
            var resolvableScope = new ResolvableServiceScope(scope, accessor);
            accessor.Scope = resolvableScope;

            return resolvableScope;
        }

        /// <summary>
        /// Now you can use the extension methods <see cref="Rougamo.Context.MethodContextExtensions.GetRootServiceProvider(Rougamo.Context.MethodContext)"/>
        /// and <see cref="Rougamo.Context.MethodContextExtensions.GetServiceProvider(Rougamo.Context.MethodContext)"/>
        /// </summary>
        public static IServiceCollection AddRougamoGenericHost(this IServiceCollection services)
        {
            services.TryAddSingleton<IServiceScopeAccessor, ServiceScopeAccessor>();
            services.AddHostedService<SetProviderHostedService>();

            return services;
        }
    }
}
