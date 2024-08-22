#if NET6_0_OR_GREATER
using System.Runtime.Loader;
#endif

using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Core.Resolving;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore
{
    internal class HttpContextLifetimeScope(HttpContext context) : ILifetimeScope
    {
        private readonly ILifetimeScope _scope = ((AutofacServiceProvider)context.RequestServices).LifetimeScope;

        public IDisposer Disposer => _scope.Disposer;

        public object Tag => _scope.Tag;

        public IComponentRegistry ComponentRegistry => _scope.ComponentRegistry;

        public event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning
        {
            add => _scope.ChildLifetimeScopeBeginning += value;
            remove => _scope.ChildLifetimeScopeBeginning -= value;
        }

        public event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding
        {
            add => _scope.CurrentScopeEnding += value;
            remove => _scope.CurrentScopeEnding -= value;
        }

        public event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning
        {
            add => _scope.ResolveOperationBeginning += value;
            remove => _scope.ResolveOperationBeginning -= value;
        }

        public object ResolveComponent(in ResolveRequest request) => _scope.ResolveComponent(in request);

        public ILifetimeScope BeginLifetimeScope() => _scope.BeginLifetimeScope();

        public ILifetimeScope BeginLifetimeScope(object tag) => _scope.BeginLifetimeScope(tag);

        public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction) => _scope.BeginLifetimeScope(configurationAction);

        public ILifetimeScope BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction) => _scope.BeginLifetimeScope(tag, configurationAction);

#if NET6_0_OR_GREATER
        public ILifetimeScope BeginLoadContextLifetimeScope(AssemblyLoadContext loadContext, Action<ContainerBuilder> configurationAction) => _scope.BeginLoadContextLifetimeScope(loadContext, configurationAction);

        public ILifetimeScope BeginLoadContextLifetimeScope(object tag, AssemblyLoadContext loadContext, Action<ContainerBuilder> configurationAction) => _scope.BeginLoadContextLifetimeScope(tag, loadContext, configurationAction);
#endif

        public void Dispose()
        {
            // Do nothing, scope is managed by HttpContext
        }

        public ValueTask DisposeAsync()
        {
            // Do nothing, scope is managed by HttpContext

            return default;
        }
    }
}
