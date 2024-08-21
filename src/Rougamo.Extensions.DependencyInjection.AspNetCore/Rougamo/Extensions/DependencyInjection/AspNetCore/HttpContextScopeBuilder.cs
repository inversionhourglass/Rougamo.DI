using Microsoft.Extensions.DependencyInjection;

namespace Rougamo.Extensions.DependencyInjection.AspNetCore
{
    internal class HttpContextScopeBuilder : HttpContextScopeBuilder<IServiceScope>
    {
        public override IServiceScope Build() => new HttpContextScope(_context!);
    }
}
