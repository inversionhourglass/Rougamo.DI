namespace System
{
    /// <summary>
    /// Get a scoped <see cref="IServiceProvider"/> if within a scope, or get the root <see cref="IServiceProvider"/>.
    /// </summary>
    public interface ISmartServiceProvider : IServiceProvider
    {
    }
}
