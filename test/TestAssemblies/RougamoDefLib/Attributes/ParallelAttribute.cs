using Rougamo.Context;
using Rougamo;

namespace RougamoDefLib.Attributes
{
    public class ParallelAttribute(int index) : MoAttribute
    {
        public override void OnEntry(MethodContext context)
        {
            var holder = context.Arguments[0] as ServiceHolder;
            if (holder == null) return;

            holder.ParallelService[index] = context.Get<ITestService>();
        }
    }
}
