using Microsoft.Extensions.DependencyInjection;
using System;

namespace RougamoDefLib
{
    public class PinnedScopeProvider(IServiceProvider provider) : IScopeProvider
    {
        public IDisposable CreateScope() => provider.CreateScope();
    }
}
