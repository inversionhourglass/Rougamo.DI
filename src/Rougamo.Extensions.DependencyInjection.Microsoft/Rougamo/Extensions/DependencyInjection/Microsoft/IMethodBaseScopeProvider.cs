using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Rougamo.Extensions.DependencyInjection.Microsoft
{
    /// <summary>
    /// <see cref="IServiceScope"/> provider
    /// </summary>
    public interface IMethodBaseScopeProvider
    {
        /// <summary>
        /// </summary>
        IServiceScope? GetScope(object @this, MethodBase method);
    }
}
