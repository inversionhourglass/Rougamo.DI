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

                    var httpScope = new TScopeBuilder().SetHttpContext(httpContextAccessor.HttpContext).Build();
                    _Scope.Value = scope = new(httpScope, null);

                    return httpScope;
                }
                return scope.Current;
            }
            set
            {
                var scope = _Scope.Value;
                if (value == null)
                {
                    if (scope == null) return;
                    _Scope.Value = scope.Parent;
                }
                else
                {
                    _Scope.Value = new(value, scope);
                }
            }
        }
    }
}
