using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Rougamo.Extensions.DependencyInjection.Microsoft
{
    /// <summary>
    /// </summary>
    public abstract class TypedScopeProvider<T> : IMethodBaseScopeProvider
    {
        /// <summary>
        /// </summary>
        public abstract Func<T, IServiceScope?> Resolve { get; }

        /// <summary>
        /// </summary>
        public IServiceScope? GetScope(object @this, MethodBase method)
        {
            if (method.IsStatic || method.DeclaringType == null || @this is not T tThis) return null;
            
            return Resolve(tThis);
        }
    }
}
