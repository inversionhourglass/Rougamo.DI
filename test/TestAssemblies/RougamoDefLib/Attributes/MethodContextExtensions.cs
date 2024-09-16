using Autofac;
using System;

namespace Rougamo.Context
{
    public static class ContextExtensions
    {
        private static Func<MethodContext, Type, object?>? _ServiceResolver;

        public static void SetMicrosoft() => _ServiceResolver = (context, type) => MethodContextExtensions.GetServiceProvider(context)?.GetService(type);

        public static void SetAutofac() => _ServiceResolver = (context, type) => context.GetAutofacCurrentScope()?.Resolve(type);

        public static void SetPinned() => _ServiceResolver = (context, type) => MethodContextPinnedExtensions.GetServiceProvider(context)?.GetService(type);

        public static T? Get<T>(this MethodContext context) where T : class
        {
            return _ServiceResolver!(context, typeof(T)) as T;
        }
    }
}
