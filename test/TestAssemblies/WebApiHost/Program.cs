using RougamoDefLib;
using System.Threading.Tasks;

namespace WebApiHost
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceHolder = new ServiceHolder();
            var main = new Main(serviceHolder);

            var hostHolder = main.Execute(serviceHolder) as Main.HostHolder;

            await hostHolder!.WaitForShutdownAsync();

            await hostHolder.DisposeAsync();
        }
    }
}
