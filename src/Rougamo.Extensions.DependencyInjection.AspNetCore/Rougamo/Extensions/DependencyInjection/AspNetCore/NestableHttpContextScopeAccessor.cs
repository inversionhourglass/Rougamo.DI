﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace Rougamo.Extensions.DependencyInjection.AspNetCore
{
    internal class NestableHttpContextScopeAccessor(IHttpContextAccessor httpContextAccessor) : IServiceScopeAccessor
    {
        private static readonly AsyncLocal<ScopeChain?> _Scope = new();

        public IServiceScope? Scope
        {
            get
            {
                var scope = _Scope.Value;

                if (scope == null)
                {
                    if (httpContextAccessor.HttpContext == null) return null;

                    _Scope.Value = scope = new();
                    var currentScope = new HttpContextScope(httpContextAccessor.HttpContext);
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
