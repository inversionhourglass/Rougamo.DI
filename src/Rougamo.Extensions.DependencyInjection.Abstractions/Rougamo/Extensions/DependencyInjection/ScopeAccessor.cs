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
                return _Scope.Value?.Current;
            }
            set
            {
                var scope = _Scope.Value;

                if (value == null)
                {
                    if (scope == null) return;
                    _Scope.Value = scope.Parent;
                }
                else if (scope?.Current != value)
                {
                    _Scope.Value = new(value, scope);
                }
            }
        }

        /// <summary>
        /// </summary>
        protected internal sealed class ScopeChain(TScope scope, ScopeChain? parent)
        {
            public TScope Current { get; } = scope;

            public ScopeChain? Parent { get; } = parent;
        }
    }
}
