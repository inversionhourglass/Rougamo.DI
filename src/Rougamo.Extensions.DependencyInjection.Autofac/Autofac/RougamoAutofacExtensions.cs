using Rougamo.Extensions.DependencyInjection.Autofac;

namespace Autofac
{
    /// <summary>
    /// </summary>
    public static class RougamoAutofacExtensions
    {
        /// <summary>
        /// Register Autofac into Rougamo
        /// </summary>
        public static ContainerBuilder RegisterRougamo(this ContainerBuilder builder)
        {
            builder.RegisterILifetimeScopeAccessor();
            builder.RegisterBuildCallback(scope => ContainerHolder.Root ??= scope);
            
            return builder;
        }

        /// <summary>
        /// Adds a default implementation for the <see cref="ILifetimeScopeAccessor"/> service.
        /// </summary>
        public static ContainerBuilder RegisterILifetimeScopeAccessor(this ContainerBuilder builder)
        {
            builder.RegisterType<LifetimeScopeAccessor>().As<ILifetimeScopeAccessor>().SingleInstance().IfNotRegistered(typeof(ILifetimeScopeAccessor));

            return builder;
        }

        /// <summary>
        /// Create a scope that can be resolved from the root container using the interface <see cref="ILifetimeScopeAccessor"/>
        /// </summary>
        public static ILifetimeScope BeginResolvableLifetimeScope(this ILifetimeScope scope)
        {
            var accessor = scope.Resolve<ILifetimeScopeAccessor>();
            var childScope = scope.BeginLifetimeScope();
            var resolvableScope = new ResolvableLifetimeScope(childScope, accessor);
            accessor.Scope = resolvableScope;

            return resolvableScope;
        }
    }
}
