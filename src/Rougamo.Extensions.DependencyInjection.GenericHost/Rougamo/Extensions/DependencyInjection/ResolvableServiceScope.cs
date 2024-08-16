using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rougamo.Extensions.DependencyInjection
{
    internal class ResolvableServiceScope(IServiceScope scope, IServiceScopeAccessor accessor) : IServiceScope
    {
        public IServiceProvider ServiceProvider => scope.ServiceProvider;

        public void Dispose()
        {
            accessor.Scope = null;

            scope.Dispose();
        }
    }
}
