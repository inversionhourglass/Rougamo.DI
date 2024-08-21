using RougamoDefLib;
using System;
using System.Threading.Tasks;

namespace GenericHost
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var main = new AutofacMain();

            var serviceHolder = new ServiceHolder();
            var hostHolder = main.Execute(serviceHolder);
            await hostHolder.WaitForExecuteAsync();
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"           outer equals: {serviceHolder.OuterService[0] == serviceHolder.OuterService[1]},\tisNull: {serviceHolder.OuterService[0] == null}");
            Console.WriteLine($"          inner1 equals: {serviceHolder.Inner1Service[0] == serviceHolder.Inner1Service[1]},\tisNull: {serviceHolder.OuterService[0] == null}");
            Console.WriteLine($"          inner2 equals: {serviceHolder.Inner2Service[0] == serviceHolder.Inner2Service[1]},\tisNull: {serviceHolder.OuterService[0] == null}");
            Console.WriteLine($"outer and inner1 equals: {serviceHolder.OuterService[0] == serviceHolder.Inner1Service[0]},\tisNull: {serviceHolder.OuterService[0] == null}");
            Console.WriteLine($"outer and inner2 equals: {serviceHolder.OuterService[0] == serviceHolder.Inner2Service[0]},\tisNull: {serviceHolder.OuterService[0] == null}");
            Console.WriteLine("---------------------------------------------");

            await hostHolder.DisposeAsync();
        }
    }
}
