using Microsoft.Extensions.DependencyInjection;

namespace Rougamo.Extensions.DependencyInjection
{
    internal class ServiceScopeAccessor : ScopeAccessor<IServiceScope>, IServiceScopeAccessor
    {
    }
}
