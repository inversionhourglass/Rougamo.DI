using Autofac;
using System;

namespace RougamoDefLib
{
    public class AutofacScopeProvider(ILifetimeScope lifetimeScope) : IScopeProvider
    {
        public IDisposable CreateScope() => lifetimeScope.BeginResolvableLifetimeScope();
    }
}
