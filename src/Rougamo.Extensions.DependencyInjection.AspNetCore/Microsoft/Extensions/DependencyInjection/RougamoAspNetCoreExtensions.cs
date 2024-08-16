using Microsoft.Extensions.DependencyInjection.Extensions;
using Rougamo.Extensions.DependencyInjection;
using Rougamo.Extensions.DependencyInjection.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// </summary>
    public static class RougamoAspNetCoreExtensions
    {
        /// <summary>
        /// Now you can use the extension methods <see cref="Rougamo.Context.MethodContextExtensions.GetRootServiceProvider(Rougamo.Context.MethodContext)"/>
        /// and <see cref="Rougamo.Context.MethodContextExtensions.GetServiceProvider(Rougamo.Context.MethodContext)"/>
        /// </summary>
        public static IServiceCollection AddRougamoAspNetCore(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddHttpContextScopeAccessor();
            services.AddRougamoGenericHost();

            return services;
        }

        /// <summary>
        /// Adds a HttpContext implementation for the <see cref="IServiceScopeAccessor"/> service.
        /// </summary>
        public static IServiceCollection AddHttpContextScopeAccessor(this IServiceCollection services)
        {
            services.TryAddSingleton<IServiceScopeAccessor, HttpContextScopeAccessor>();

            return services;
        }

        /// <summary>
        /// Adds a nestable HttpContext implementation for the <see cref="IServiceScopeAccessor"/> service.
        /// </summary>
        public static IServiceCollection AddNestableHttpContextScopeAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IServiceScopeAccessor, NestableHttpContextScopeAccessor>();

            return services;
        }
    }
}
