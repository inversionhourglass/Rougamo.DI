using Autofac;

namespace Rougamo.Extensions.DependencyInjection.Autofac
{
    /// <summary>
    /// Provides access to the current <see cref="ILifetimeScope"/> if it exists.
    /// </summary>
    public interface ILifetimeScopeAccessor : IScopeAccessor<ILifetimeScope>
    {
    }
}
