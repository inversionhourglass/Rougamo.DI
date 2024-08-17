using Microsoft.Extensions.DependencyInjection;
using Rougamo;
using Rougamo.Context;

namespace RougamoDefLib.Attributes
{
    public class OuterAttribute(int index) : MoAttribute
    {
        public override void OnEntry(MethodContext context)
        {
            var holder = context.Arguments[0] as ServiceHolder;
            var provider = context.GetServiceProvider();
            if (holder == null || provider == null) return;

            holder.OuterService[index] = provider.GetService<ITestService>();
        }
    }
}
