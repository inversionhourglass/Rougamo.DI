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
        /// Now you can use the extension methods <see cref="Rougamo.Context.MethodContextExtensions.GetRootServiceProvider(Rougamo.Context.MethodContext)"/>
        /// and <see cref="Rougamo.Context.MethodContextExtensions.GetServiceProvider(Rougamo.Context.MethodContext)"/>
        /// </summary>
        public static IServiceCollection AddRougamoGenericHost(this IServiceCollection services)
        {
            return services
                    .AddServiceScopeAccessor()
                    .AddHostedService<SetProviderHostedService>();
        }

        /// <summary>
        /// Adds a default implementation for the <see cref="IServiceScopeAccessor"/> service.
        /// </summary>
        public static IServiceCollection AddServiceScopeAccessor(this IServiceCollection services)
        {
            services.TryAddSingleton<IServiceScopeAccessor, ServiceScopeAccessor>();
            services.TryAddSingleton<ISmartServiceProvider, SmartServiceProvider>();

            return services;
        }
    }
}
