using System;

namespace RougamoDefLib
{
    public class MicrosoftScopeProvider(IServiceProvider provider) : IScopeProvider
    {
        public IDisposable CreateScope() => provider.CreateResolvableScope();
    }
}
