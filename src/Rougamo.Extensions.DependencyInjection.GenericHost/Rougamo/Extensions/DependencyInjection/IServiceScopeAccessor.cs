using Microsoft.Extensions.DependencyInjection;

namespace Rougamo.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides access to the current <see cref="IServiceScope"/> if it exists.
    /// </summary>
    public interface IServiceScopeAccessor
    {
        /// <summary>
        /// Get current scope. Return null if not within a scope.
        /// </summary>
        IServiceScope? Scope { get; set; }
    }
}
