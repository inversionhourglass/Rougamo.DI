using Rougamo;
using Rougamo.Context;

namespace RougamoDefLib.Attributes
{
    public class OuterAttribute(int index) : MoAttribute
    {
        public override void OnEntry(MethodContext context)
        {
            var holder = context.Arguments[0] as ServiceHolder;
            if (holder == null) return;

            holder.OuterService[index] = context.Get<ITestService>();
        }
    }
}
