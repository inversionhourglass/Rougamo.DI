using System.Collections.Generic;
using System.Threading;

namespace Rougamo.Extensions.DependencyInjection
{
    /// <summary>
    /// <inheritdoc cref="IScopeAccessor{TScope}" />
    /// </summary>
    public class ScopeAccessor<TScope> : IScopeAccessor<TScope> where TScope : class
    {
        /// <summary>
        /// </summary>
        protected static readonly AsyncLocal<ScopeChain?> _Scope = new();

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public virtual TScope? Scope
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

        /// <summary>
        /// </summary>
        protected class ScopeChain : Stack<TScope>
        {
            /// <summary>
            /// </summary>
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
