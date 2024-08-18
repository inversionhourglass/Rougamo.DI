using Rougamo.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace RougamoDefLib
{
    public static class ServiceProviderHolderAccessor
    {
        private static readonly FieldInfo _FieldRoot;

        static ServiceProviderHolderAccessor()
        {
            var type = typeof(IServiceScopeAccessor).Assembly.DefinedTypes.Single(x => x.Name == "ServiceProviderHolder");
            _FieldRoot = type.GetField("Root");
        }

        public static void SetRootNull()
        {
            _FieldRoot.SetValue(null, null);
        }
    }
}
