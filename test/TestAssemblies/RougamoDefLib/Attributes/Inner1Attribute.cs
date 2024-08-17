using Rougamo.Context;
using Rougamo;
using Microsoft.Extensions.DependencyInjection;

namespace RougamoDefLib.Attributes
{
    public class Inner1Attribute(int index) : MoAttribute
    {
        public override void OnEntry(MethodContext context)
        {
            var holder = context.Arguments[0] as ServiceHolder;
            var provider = context.GetServiceProvider();
            if (holder == null || provider == null) return;

            holder.Inner1Service[index] = provider.GetService<ITestService>();
        }
    }
}
