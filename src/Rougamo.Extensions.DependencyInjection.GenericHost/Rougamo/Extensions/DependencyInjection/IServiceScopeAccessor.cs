using Microsoft.Extensions.DependencyInjection;

namespace Rougamo.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides access to the current <see cref="IServiceScope"/> if it exists.
    /// </summary>
    public interface IServiceScopeAccessor : IScopeAccessor<IServiceScope>
    {
    }
}
