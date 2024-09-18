using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Rougamo.Extensions.DependencyInjection.Microsoft
{
    /// <summary>
    /// </summary>
    public abstract class SpecificPropertyScopeProvider : IMethodBaseScopeProvider
    {
        private static readonly ConcurrentDictionary<(Type declaringType, string propertyName), Func<object, IServiceScope?>> _Cache = [];

        /// <summary>
        /// </summary>
        public abstract string PropertyName { get; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public virtual IServiceScope? GetScope(object @this, MethodBase method)
        {
            var declaringType = method.DeclaringType;
            if (declaringType == null) return null;

            var resolve = _Cache.GetOrAdd((declaringType, PropertyName), Resolve);
            return resolve(@this);
        }

        private static Func<object, IServiceScope?> Resolve((Type, string) key) => Resolve<IServiceScope>(key);

        /// <summary>
        /// </summary>
        protected static Func<object, T?> Resolve<T>((Type, string) key)
        {
            (var declaringType, var fieldName) = key;

            var propertyInfo = declaringType.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (propertyInfo == null || !typeof(T).IsAssignableFrom(propertyInfo.PropertyType)) return Default;

            var pObjThis = Expression.Parameter(typeof(object), "@this");
            var convert = Expression.Convert(pObjThis, declaringType);
            var getter = Expression.Call(convert, propertyInfo.GetMethod);
            var expression = propertyInfo.PropertyType == typeof(T) ? (Expression)getter : Expression.Convert(getter, typeof(T));
            var lambda = Expression.Lambda<Func<object, T?>>(expression, pObjThis);

            return lambda.Compile();

            static T? Default(object @this) => default;
        }
    }
}
