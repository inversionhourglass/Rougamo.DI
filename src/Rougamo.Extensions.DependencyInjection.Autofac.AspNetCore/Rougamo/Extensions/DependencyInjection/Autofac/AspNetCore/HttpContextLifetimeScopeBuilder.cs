using Autofac;
using Rougamo.Extensions.DependencyInjection.AspNetCore;

namespace Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore
{
    internal class HttpContextLifetimeScopeBuilder : HttpContextScopeBuilder<ILifetimeScope>
    {
        public override ILifetimeScope Build() => new HttpContextLifetimeScope(_context!);
    }
}
