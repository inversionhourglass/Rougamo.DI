using Microsoft.AspNetCore.Http;
using System;

namespace Rougamo.Extensions.DependencyInjection.AspNetCore
{
    /// <summary>
    /// Do not manage the scope, just get the scope from HttpContext.
    /// </summary>
    public class BaseHttpContextScopeAccessor<TScopeBuilder, TScope>(IHttpContextAccessor httpContextAccessor) where TScopeBuilder : HttpContextScopeBuilder<TScope>, new()
    {
        /// <summary>
        /// The scope of current HttpContext
        /// </summary>
        public TScope? Scope
        {
            get => httpContextAccessor.HttpContext == null ? default : new TScopeBuilder().SetHttpContext(httpContextAccessor.HttpContext).Build();
            set => throw new InvalidOperationException("Cannot create a nested scope within an HttpContext scope.");
        }
    }
}
