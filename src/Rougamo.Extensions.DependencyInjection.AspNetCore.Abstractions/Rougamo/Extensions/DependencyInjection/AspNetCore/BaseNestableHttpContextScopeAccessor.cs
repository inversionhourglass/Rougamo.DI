using Microsoft.AspNetCore.Http;

namespace Rougamo.Extensions.DependencyInjection.AspNetCore
{
    /// <summary>
    /// <inheritdoc cref="ScopeAccessor{TScope}" />
    /// </summary>
    public class BaseNestableHttpContextScopeAccessor<TScopeBuilder, TScope>(IHttpContextAccessor httpContextAccessor): ScopeAccessor<TScope> where TScopeBuilder : HttpContextScopeBuilder<TScope>, new() where TScope : class
    {
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override TScope? Scope
        {
            get
            {
                var scope = _Scope.Value;

                if (scope == null)
                {
                    if (httpContextAccessor.HttpContext == null) return null;

                    _Scope.Value = scope = new();
                    var currentScope = new TScopeBuilder().SetHttpContext(httpContextAccessor.HttpContext).Build();
                    scope.Push(currentScope);

                    return currentScope;
                }
                return scope.Peek();
            }
            set
            {
                if (value == null)
                {
                    var scope = _Scope.Value;
                    if (scope == null) return;
                    if (!scope.TryPop()) _Scope.Value = null;
                }
                else
                {
                    if (Scope == null)
                    {
                        _Scope.Value = new();
                    }
                    _Scope.Value!.Push(value);
                }
            }
        }
    }
}
