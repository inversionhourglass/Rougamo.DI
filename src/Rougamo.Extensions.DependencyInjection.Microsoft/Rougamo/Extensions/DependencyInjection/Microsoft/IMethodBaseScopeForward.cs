using Microsoft.Extensions.DependencyInjection;

namespace Rougamo.Extensions.DependencyInjection.Microsoft
{
    /// <summary>
    /// Preferentially obtain the <see cref="IServiceScope"/> from the <see cref="IMethodBaseScopeForward"/>s,
    /// return the default <see cref="IServiceScope"/> if all the instances return null.
    /// </summary>
    public interface IMethodBaseScopeForward : IMethodBaseScopeProvider
    {

    }
}
