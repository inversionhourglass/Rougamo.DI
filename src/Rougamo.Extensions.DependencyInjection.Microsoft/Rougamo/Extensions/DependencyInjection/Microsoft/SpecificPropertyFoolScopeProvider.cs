using DependencyInjection.StaticAccessor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Rougamo.Extensions.DependencyInjection.Microsoft
{
    /// <summary>
    /// </summary>
    public abstract class SpecificPropertyFoolScopeProvider : SpecificPropertyScopeProvider
    {
        private static readonly ConcurrentDictionary<(Type declaringType, string fieldName), Func<object, IServiceProvider?>> _Cache = [];

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override IServiceScope? GetScope(object @this, MethodBase method)
        {
            var declaringType = method.DeclaringType;
            if (declaringType == null) return null;

            var resolve = _Cache.GetOrAdd((declaringType, PropertyName), Resolve);
            var serviceProvider = resolve(@this);
            return serviceProvider == null ? null : new FoolScope(serviceProvider);
        }

        private static Func<object, IServiceProvider?> Resolve((Type, string) key) => Resolve<IServiceProvider>(key);
    }
}
