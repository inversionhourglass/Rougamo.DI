using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rougamo.Extensions.DependencyInjection
{
    internal class SmartServiceProvider(IServiceProvider serviceProvider) : ISmartServiceProvider
    {
        private readonly IServiceScopeAccessor? _scopeAccessor = serviceProvider.GetService<IServiceScopeAccessor>();

        public object? GetService(Type serviceType)
        {
            var provider = _scopeAccessor?.Scope?.ServiceProvider ?? serviceProvider;
            return provider.GetService(serviceType);
        }
    }
}
