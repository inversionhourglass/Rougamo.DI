using Autofac;
using Microsoft.AspNetCore.Http;
using Rougamo.Extensions.DependencyInjection.AspNetCore;

namespace Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore
{
    internal class HttpContextLifetimeScopeAccessor(IHttpContextAccessor httpContextAccessor) : BaseHttpContextScopeAccessor<HttpContextLifetimeScopeBuilder, ILifetimeScope>(httpContextAccessor), ILifetimeScopeAccessor
    {
    }
}
