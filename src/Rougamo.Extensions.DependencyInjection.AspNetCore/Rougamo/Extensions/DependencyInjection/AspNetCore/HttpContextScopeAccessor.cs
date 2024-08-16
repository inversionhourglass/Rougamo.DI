using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rougamo.Extensions.DependencyInjection.AspNetCore
{
    internal class HttpContextScopeAccessor(IHttpContextAccessor httpContextAccessor) : IServiceScopeAccessor
    {
        public IServiceScope? Scope
        {
            get => httpContextAccessor.HttpContext == null ? null : new HttpContextScope(httpContextAccessor.HttpContext);
            set => throw new InvalidOperationException("Cannot set the scope in asp.net core project.");
        }
    }
}
