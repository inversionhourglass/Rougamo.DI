using Rougamo.Context;
using Rougamo;
using Microsoft.Extensions.DependencyInjection;

namespace RougamoDefLib.Attributes
{
    public class Inner2Attribute(int index) : MoAttribute
    {
        public override void OnEntry(MethodContext context)
        {
            var holder = context.Arguments[0] as ServiceHolder;
            var provider = context.GetServiceProvider();
            if (holder == null || provider == null) return;

            holder.Inner2Service[index] = provider.GetService<ITestService>();
        }
    }
}
