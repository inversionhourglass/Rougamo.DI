using Microsoft.Extensions.DependencyInjection;
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
    }
}
