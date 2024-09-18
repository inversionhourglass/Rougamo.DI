using DependencyInjection.StaticAccessor;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rougamo.Extensions.DependencyInjection.Microsoft
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TypedFoolScopeProvider<T> : TypedScopeProvider<T>
    {
        /// <summary>
        /// </summary>
        public abstract Func<T, IServiceProvider?> ServiceProviderResolver { get; }

        /// <summary>
        /// </summary>
        public override Func<T, IServiceScope?> Resolve => ResolveCore;

        private IServiceScope? ResolveCore(T service)
        {
            var serviceProvider = ServiceProviderResolver(service);
            return serviceProvider == null ? null : new FoolScope(serviceProvider);
        }
    }
}
