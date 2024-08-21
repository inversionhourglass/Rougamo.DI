using RougamoDefLib;
using System.Threading.Tasks;

namespace WebApiHost
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceHolder = new ServiceHolder();
            var main = new AutofacMain();//new Main();

            var hostHolder = main.Execute(serviceHolder);

            await hostHolder.WaitForShutdownAsync();

            await hostHolder.DisposeAsync();
        }
    }
}
