using Autofac;
using System;

namespace Rougamo.Context
{
    public static class ContextExtensions
    {
        private static Func<MethodContext, Type, object?>? _ServiceResolver;

        public static void SetMicrosoft() => _ServiceResolver = (context, type) => context.GetServiceProvider()?.GetService(type);

        public static void SetAutofac() => _ServiceResolver = (context, type) => context.GetAutofacCurrentScope()?.Resolve(type);

        public static T? Get<T>(this MethodContext context) where T : class
        {
            return _ServiceResolver!(context, typeof(T)) as T;
        }
    }
}
