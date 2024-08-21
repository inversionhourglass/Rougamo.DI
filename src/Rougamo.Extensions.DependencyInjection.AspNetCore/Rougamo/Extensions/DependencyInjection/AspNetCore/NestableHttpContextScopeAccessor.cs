using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Rougamo.Extensions.DependencyInjection.AspNetCore
{
    internal class NestableHttpContextScopeAccessor(IHttpContextAccessor httpContextAccessor) : BaseNestableHttpContextScopeAccessor<HttpContextScopeBuilder, IServiceScope>(httpContextAccessor), IServiceScopeAccessor
    {
    }
}
