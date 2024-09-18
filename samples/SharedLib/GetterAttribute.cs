using Rougamo;
using Rougamo.Context;

namespace SharedLib
{
    public class GetterAttribute : MoAttribute
    {
        public override Feature Features => Feature.EntryReplace;

        public override void OnEntry(MethodContext context)
        {
            var service = context.GetRequiredService<TestService>();

            context.ReplaceReturnValue(this, service);
        }
    }
}
