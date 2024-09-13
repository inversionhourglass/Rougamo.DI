using Microsoft.AspNetCore.Mvc;
using RougamoDefLib.Attributes;
using RougamoDefLib;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading;

namespace WebApiHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController(IScopeProvider provider, ServiceHolder serviceHolder, IHttpContextAccessor accesor) : ControllerBase
    {
        [HttpGet]
        public async Task<string> Get()
        {
            var httpContext = accesor.HttpContext;
            Outer1(serviceHolder);

            using (var innerScope1 = provider.CreateScope())
            {
                Inner11(serviceHolder);
                Inner12(serviceHolder);
            }

            using (var innerScope2 = provider.CreateScope())
            {
                Inner21(serviceHolder);
                Inner22(serviceHolder);
            }

            var locker1 = new EventWaitHandle(false, EventResetMode.ManualReset);
            var locker2 = new EventWaitHandle(false, EventResetMode.ManualReset);
            var t1 = Task.Run(() =>
            {
                locker1.WaitOne();
                Parallel1(serviceHolder);
                locker2.Set();
            });
            var t2 = Task.Run(() =>
            {
                using (var pScope = provider.CreateScope())
                {
                    Parallel2(serviceHolder);
                    locker1.Set();
                    locker2.WaitOne();
                }
            });
            await Task.WhenAll(t1, t2);

            Outer2(serviceHolder);

            return BuildResult(serviceHolder);

            string BuildResult(ServiceHolder serviceHolder)
            {
                var builder = new StringBuilder();
                builder.AppendLine($"           outer equals: {serviceHolder.OuterService[0] == serviceHolder.OuterService[1]},\tisNull: {serviceHolder.OuterService[0] == null}");
                builder.AppendLine($"          inner1 equals: {serviceHolder.Inner1Service[0] == serviceHolder.Inner1Service[1]},\tisNull: {serviceHolder.OuterService[0] == null}");
                builder.AppendLine($"          inner2 equals: {serviceHolder.Inner2Service[0] == serviceHolder.Inner2Service[1]},\tisNull: {serviceHolder.OuterService[0] == null}");
                builder.AppendLine($"outer and inner1 equals: {serviceHolder.OuterService[0] == serviceHolder.Inner1Service[0]},\tisNull: {serviceHolder.OuterService[0] == null}");
                builder.AppendLine($"outer and inner2 equals: {serviceHolder.OuterService[0] == serviceHolder.Inner2Service[0]},\tisNull: {serviceHolder.OuterService[0] == null}");
                return builder.ToString();
            }
        }

        [Outer(0)]
        private void Outer1(ServiceHolder serviceHolder) { }

        [Outer(1)]
        private void Outer2(ServiceHolder serviceHolder) { }

        [Inner1(0)]
        private void Inner11(ServiceHolder serviceHolder) { }

        [Inner1(1)]
        private void Inner12(ServiceHolder serviceHolder) { }

        [Inner2(0)]
        private void Inner21(ServiceHolder serviceHolder) { }

        [Inner2(1)]
        private void Inner22(ServiceHolder serviceHolder) { }

        [Parallel(0)]
        private void Parallel1(ServiceHolder serviceHolder) { }

        [Parallel(1)]
        private void Parallel2(ServiceHolder serviceHolder) { }
    }
}
