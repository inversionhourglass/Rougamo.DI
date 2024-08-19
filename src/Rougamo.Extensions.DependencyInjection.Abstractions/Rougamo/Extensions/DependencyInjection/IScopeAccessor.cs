namespace Rougamo.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides access to the current <see cref="Scope"/> if it exists.
    /// </summary>
    public interface IScopeAccessor<TScope> where TScope : class
    {
        /// <summary>
        /// Get current scope. Return null if not within a scope.
        /// </summary>
        TScope? Scope { get; set; }
    }
}
