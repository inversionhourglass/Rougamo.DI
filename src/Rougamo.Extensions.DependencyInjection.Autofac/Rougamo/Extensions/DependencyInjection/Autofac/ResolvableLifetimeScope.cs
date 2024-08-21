using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Core.Resolving;
using System;
using System.Threading.Tasks;

namespace Rougamo.Extensions.DependencyInjection.Autofac
{
    internal class ResolvableLifetimeScope(ILifetimeScope scope, ILifetimeScopeAccessor accessor) : ILifetimeScope
    {
        public IDisposer Disposer => scope.Disposer;

        public object Tag => scope.Tag;

        public IComponentRegistry ComponentRegistry => scope.ComponentRegistry;

        public event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning
        {
            add => scope.ChildLifetimeScopeBeginning += value;
            remove => scope.ChildLifetimeScopeBeginning -= value;
        }

        public event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding
        {
            add => scope.CurrentScopeEnding += value;
            remove => scope.CurrentScopeEnding -= value;
        }

        public event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning
        {
            add => scope.ResolveOperationBeginning += value;
            remove => scope.ResolveOperationBeginning -= value;
        }

        public object ResolveComponent(ResolveRequest request) => scope.ResolveComponent(request);

        public ILifetimeScope BeginLifetimeScope() => scope.BeginLifetimeScope();

        public ILifetimeScope BeginLifetimeScope(object tag) => scope.BeginLifetimeScope(tag);

        public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction) => scope.BeginLifetimeScope(configurationAction);

        public ILifetimeScope BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction) => scope.BeginLifetimeScope(tag, configurationAction);

        public void Dispose()
        {
            accessor.Scope = null;

            scope.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            accessor.Scope = null;

            return scope.DisposeAsync();
        }
    }
}
