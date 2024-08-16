using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rougamo.Extensions.DependencyInjection.AspNetCore
{
    internal class HttpContextScope(HttpContext context) : IServiceScope
    {
        public IServiceProvider ServiceProvider => context.RequestServices;

        public void Dispose()
        {
            // Do nothing, scope is managed by HttpContext
        }
    }
}
