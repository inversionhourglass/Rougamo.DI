using Rougamo.Extensions.DependencyInjection.Autofac;
using System.Linq;
using System.Reflection;

namespace RougamoDefLib
{
    public static class ContainerHolderAccessor
    {
        private static readonly FieldInfo _FieldRoot;

        static ContainerHolderAccessor()
        {
            var type = typeof(ILifetimeScopeAccessor).Assembly.DefinedTypes.Single(x => x.Name == "ContainerHolder");
            _FieldRoot = type.GetField("Root");
        }

        public static void SetRootNull()
        {
            _FieldRoot.SetValue(null, null);
        }
    }
}
