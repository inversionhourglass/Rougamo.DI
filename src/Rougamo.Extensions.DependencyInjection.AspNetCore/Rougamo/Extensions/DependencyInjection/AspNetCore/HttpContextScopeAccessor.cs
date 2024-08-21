using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Rougamo.Extensions.DependencyInjection.AspNetCore
{
    internal class HttpContextScopeAccessor(IHttpContextAccessor httpContextAccessor) : BaseHttpContextScopeAccessor<HttpContextScopeBuilder, IServiceScope>(httpContextAccessor), IServiceScopeAccessor
    {
    }
}
