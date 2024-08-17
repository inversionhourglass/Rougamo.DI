using Microsoft.AspNetCore.Mvc;
using RougamoDefLib.Attributes;
using RougamoDefLib;
using System;
using System.Text;

namespace WebApiHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController(IServiceProvider provider, ServiceHolder serviceHolder) : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            Outer1(serviceHolder);

            using (var innerScope1 = provider.CreateResolvableScope())
            {
                Inner11(serviceHolder);
                Inner12(serviceHolder);
            }

            using (var innerScope2 = provider.CreateResolvableScope())
            {
                Inner21(serviceHolder);
                Inner22(serviceHolder);
            }

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
    }
}
