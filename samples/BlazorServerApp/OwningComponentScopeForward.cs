using Rougamo.Extensions.DependencyInjection.Microsoft;

namespace BlazorServerApp
{
    public class OwningComponentScopeForward : SpecificPropertyFoolScopeProvider, IMethodBaseScopeForward
    {
        public override string PropertyName => "ScopedServices";
    }
}
