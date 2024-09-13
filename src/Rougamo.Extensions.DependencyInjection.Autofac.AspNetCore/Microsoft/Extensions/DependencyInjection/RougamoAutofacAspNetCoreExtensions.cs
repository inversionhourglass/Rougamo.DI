using Autofac;
using Rougamo.Extensions.DependencyInjection.Autofac;
using Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// </summary>
    public static class RougamoAutofacAspNetCoreExtensions
    {
        /// <summary>
        /// Now you can use the extension methods <see cref="Rougamo.Context.RougamAutofacExtensions.GetAutofacRootScope(Rougamo.Context.MethodContext)"/>
        /// and <see cref="Rougamo.Context.RougamAutofacExtensions.GetAutofacCurrentScope(Rougamo.Context.MethodContext)"/>
        /// </summary>
        public static ContainerBuilder RegisterRougamoAspNetCore(this ContainerBuilder builder)
        {
            builder.RegisterAutofacNestableHttpContextScopeAccessor();
            builder.RegisterRougamo();

            return builder;
        }

        /// <summary>
        /// Adds a HttpContext implementation for the <see cref="ILifetimeScopeAccessor"/> service.
        /// </summary>
        public static ContainerBuilder RegisterAutofacHttpContextScopeAccessor(this ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextLifetimeScopeAccessor>().As<ILifetimeScopeAccessor>().SingleInstance().IfNotRegistered(typeof(ILifetimeScopeAccessor));

            return builder;
        }

        /// <summary>
        /// Adds a nestable HttpContext implementation for the <see cref="ILifetimeScopeAccessor"/> service.
        /// </summary>
        public static ContainerBuilder RegisterAutofacNestableHttpContextScopeAccessor(this ContainerBuilder builder)
        {
            builder.RegisterType<NestableHttpContextLifetimeScopeAccessor>().As<ILifetimeScopeAccessor>().SingleInstance().IfNotRegistered(typeof(ILifetimeScopeAccessor));

            return builder;
        }
    }
}
