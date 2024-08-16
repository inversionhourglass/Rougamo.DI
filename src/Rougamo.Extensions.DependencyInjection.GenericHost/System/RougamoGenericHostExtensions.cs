using Microsoft.Extensions.DependencyInjection;
using Rougamo.Extensions.DependencyInjection;

namespace System
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
    }
}
