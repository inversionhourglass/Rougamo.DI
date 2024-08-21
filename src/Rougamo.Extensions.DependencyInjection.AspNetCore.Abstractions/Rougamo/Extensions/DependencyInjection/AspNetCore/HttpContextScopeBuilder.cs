using Microsoft.AspNetCore.Http;

namespace Rougamo.Extensions.DependencyInjection.AspNetCore
{
    /// <summary>
    /// HttpContext scope builder
    /// </summary>
    public abstract class HttpContextScopeBuilder<TScope>
    {
        /// <summary>
        /// </summary>
        protected HttpContext? _context;

        /// <summary>
        /// Set the current HttpContext
        /// </summary>
        public virtual HttpContextScopeBuilder<TScope> SetHttpContext(HttpContext httpContext)
        {
            _context = httpContext;

            return this;
        }

        /// <summary>
        /// Build the scope
        /// </summary>
        public abstract TScope Build();
    }
}
