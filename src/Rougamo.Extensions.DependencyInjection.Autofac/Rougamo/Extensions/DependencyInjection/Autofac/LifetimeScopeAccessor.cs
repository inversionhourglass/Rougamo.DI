using Autofac;

namespace Rougamo.Extensions.DependencyInjection.Autofac
{
    internal class LifetimeScopeAccessor : ScopeAccessor<ILifetimeScope>, ILifetimeScopeAccessor
    {
    }
}
