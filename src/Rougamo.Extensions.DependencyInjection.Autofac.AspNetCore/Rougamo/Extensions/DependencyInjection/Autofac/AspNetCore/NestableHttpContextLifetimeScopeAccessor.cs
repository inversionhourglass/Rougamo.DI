using Autofac;
using Microsoft.AspNetCore.Http;
using Rougamo.Extensions.DependencyInjection.AspNetCore;

namespace Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore
{
    internal class NestableHttpContextLifetimeScopeAccessor(IHttpContextAccessor httpContextAccessor) : BaseNestableHttpContextScopeAccessor<HttpContextLifetimeScopeBuilder, ILifetimeScope>(httpContextAccessor), ILifetimeScopeAccessor
    {
    }
}
