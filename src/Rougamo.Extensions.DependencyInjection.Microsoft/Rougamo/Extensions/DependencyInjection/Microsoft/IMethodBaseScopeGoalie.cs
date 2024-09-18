using DependencyInjection.StaticAccessor;

namespace Rougamo.Extensions.DependencyInjection.Microsoft
{
    /// <summary>
    /// If the default scope value of <see cref="PinnedScope.Scope"/> is null, then try to get the scope from <see cref="IMethodBaseScopeGoalie"/> instances.
    /// </summary>
    public interface IMethodBaseScopeGoalie : IMethodBaseScopeProvider
    {

    }
}
