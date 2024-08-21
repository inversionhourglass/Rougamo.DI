using Autofac;
using Rougamo.Extensions.DependencyInjection.Autofac;

namespace Rougamo.Context
{
    /// <summary>
    /// </summary>
    public static class RougamAutofacExtensions
    {
        /// <summary>
        /// Get the root <see cref="ILifetimeScope"/>
        /// </summary>
        public static ILifetimeScope? GetAutofacRootScope(this MethodContext context)
        {
            return ContainerHolder.Root;
        }

        /// <summary>
        /// Get the current scope <see cref="ILifetimeScope"/>, return root <see cref="ILifetimeScope"/> if it is not within a scope.
        /// </summary>
        public static ILifetimeScope? GetAutofacCurrentScope(this MethodContext context)
        {
            var container = ContainerHolder.Root;
            if (container == null) return null;

            var accessor = container.Resolve<ILifetimeScopeAccessor>();

            return accessor?.Scope ?? container;
        }
    }
}
