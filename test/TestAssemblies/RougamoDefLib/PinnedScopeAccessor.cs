using DependencyInjection.StaticAccessor;
using System.Reflection;

namespace RougamoDefLib
{
    public static class PinnedScopeAccessor
    {
        private static readonly MethodInfo _RootSetter;

        static PinnedScopeAccessor()
        {
            var prop = typeof(PinnedScope).GetProperty("RootServices", BindingFlags.Public | BindingFlags.Static);
            _RootSetter = prop.SetMethod;
        }

        public static void SetRootNull()
        {
            _RootSetter.Invoke(null, [null]);
        }
    }
}
