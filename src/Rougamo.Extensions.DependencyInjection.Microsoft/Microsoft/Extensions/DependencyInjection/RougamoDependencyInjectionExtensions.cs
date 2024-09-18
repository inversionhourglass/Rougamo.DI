using Rougamo.Extensions.DependencyInjection.Microsoft;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// </summary>
    public static class RougamoDependencyInjectionExtensions
    {
        /// <summary>
        /// </summary>
        public static IServiceCollection AddMethodBaseScopeForward<T>(this IServiceCollection services) where T : class, IMethodBaseScopeForward
        {
            return services.AddSingleton<IMethodBaseScopeForward, T>();
        }

        /// <summary>
        /// </summary>
        public static IServiceCollection AddMethodBaseScopeForward(this IServiceCollection services, Type forwardType)
        {
            return services.AddSingleton(typeof(IMethodBaseScopeForward), forwardType);
        }

        /// <summary>
        /// </summary>
        public static IServiceCollection AddMethodBaseScopeGoalie<T>(this IServiceCollection services) where T : class, IMethodBaseScopeGoalie
        {
            return services.AddSingleton<IMethodBaseScopeGoalie, T>();
        }

        /// <summary>
        /// </summary>
        public static IServiceCollection AddMethodBaseScopeGoalie(this IServiceCollection services, Type goalieType)
        {
            return services.AddSingleton(typeof(IMethodBaseScopeGoalie), goalieType);
        }
    }
}
