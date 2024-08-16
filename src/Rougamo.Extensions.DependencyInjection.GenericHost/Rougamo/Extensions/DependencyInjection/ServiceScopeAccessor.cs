using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading;

namespace Rougamo.Extensions.DependencyInjection
{
    internal class ServiceScopeAccessor : IServiceScopeAccessor
    {
        private static readonly AsyncLocal<ScopeChain?> _Scope = new();

        public IServiceScope? Scope
        {
            get
            {
                return _Scope.Value?.Peek();
            }
            set
            {
                var scope = _Scope.Value;

                if (value == null)
                {
                    if (scope == null) return;
                    if (!scope.TryPop()) _Scope.Value = null;
                }
                else
                {
                    if (scope == null)
                    {
                        _Scope.Value = scope = new();
                    }
                    scope.Push(value);
                }
            }
        }

        private sealed class ScopeChain : Stack<IServiceScope>
        {
            public bool TryPop()
            {
#if NETSTANDARD2_0
                if (Count > 0)
                {
                    Pop();
                    return true;
                }
                return false;
#else
                return TryPop(out _);
#endif
            }
        }
    }
}
