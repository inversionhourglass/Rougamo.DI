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
            services.AddSingleton<IServiceScopeAccessor, HttpContextScopeAccessor>();
            services.AddRougamoGenericHost();

            return services;
        }
    }
}
