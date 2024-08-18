#if NETCOREAPP3_1_OR_GREATER
using RougamoDefLib;
using WebApiHost;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using System;

namespace Rougamo.DITests
{
    public class WebApiHostTests
    {
        [Fact]
        public async Task NormalTest()
        {
            var serviceHolder = new ServiceHolder();
            var main = new Main();
            var hostHolder = main.Execute(serviceHolder);

            var response = await SendTestRequestAsync(hostHolder.Address);

            Assert.Contains(nameof(InvalidOperationException), response);

            Assert.NotNull(serviceHolder.OuterService[0]);
            Assert.Null(serviceHolder.OuterService[1]);
            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.OuterService[1]);

            Assert.Null(serviceHolder.Inner1Service[0]);
            Assert.Equal(serviceHolder.Inner1Service[0], serviceHolder.Inner1Service[1]);

            Assert.Null(serviceHolder.Inner2Service[0]);
            Assert.Equal(serviceHolder.Inner2Service[0], serviceHolder.Inner2Service[1]);

            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.Inner1Service[0]);
            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.Inner2Service[0]);

            await hostHolder.StopAsync();
            await hostHolder.DisposeAsync();
        }

        [Fact]
        public async Task NestableScopeTest()
        {
            var serviceHolder = new ServiceHolder();
            var main = new Main();
            var hostHolder = main.ExecuteNestableScope(serviceHolder);

            var response = await SendTestRequestAsync(hostHolder.Address);

            Assert.DoesNotContain(nameof(InvalidOperationException), response);

            Assert.NotNull(serviceHolder.OuterService[0]);
            Assert.Equal(serviceHolder.OuterService[0], serviceHolder.OuterService[1]);

            Assert.NotNull(serviceHolder.Inner1Service[0]);
            Assert.Equal(serviceHolder.Inner1Service[0], serviceHolder.Inner1Service[1]);

            Assert.NotNull(serviceHolder.Inner2Service[0]);
            Assert.Equal(serviceHolder.Inner2Service[0], serviceHolder.Inner2Service[1]);

            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.Inner1Service[0]);
            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.Inner2Service[0]);

            await hostHolder.StopAsync();
            await hostHolder.DisposeAsync();
        }

        [Fact]
        public async Task WithoutRougamoTest()
        {
            var serviceHolder = new ServiceHolder();
            var main = new Main();
            var hostHolder = main.ExecuteWithoutRougamo(serviceHolder);

            var response = await SendTestRequestAsync(hostHolder.Address);

            Assert.DoesNotContain(nameof(InvalidOperationException), response);

            Assert.Null(serviceHolder.OuterService[0]);
            Assert.Equal(serviceHolder.OuterService[0], serviceHolder.OuterService[1]);

            Assert.Null(serviceHolder.Inner1Service[0]);
            Assert.Equal(serviceHolder.Inner1Service[0], serviceHolder.Inner1Service[1]);

            Assert.Null(serviceHolder.Inner2Service[0]);
            Assert.Equal(serviceHolder.Inner2Service[0], serviceHolder.Inner2Service[1]);

            Assert.Equal(serviceHolder.OuterService[0], serviceHolder.Inner1Service[0]);
            Assert.Equal(serviceHolder.OuterService[0], serviceHolder.Inner2Service[0]);

            await hostHolder.StopAsync();
            await hostHolder.DisposeAsync();
        }

        [Fact]
        public async Task TransientTest()
        {
            var serviceHolder = new ServiceHolder();
            var main = new Main();
            var hostHolder = main.ExecuteTransient(serviceHolder);

            var response = await SendTestRequestAsync(hostHolder.Address);

            Assert.DoesNotContain(nameof(InvalidOperationException), response);

            Assert.NotNull(serviceHolder.OuterService[0]);
            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.OuterService[1]);

            Assert.NotNull(serviceHolder.Inner1Service[0]);
            Assert.NotEqual(serviceHolder.Inner1Service[0], serviceHolder.Inner1Service[1]);

            Assert.NotNull(serviceHolder.Inner2Service[0]);
            Assert.NotEqual(serviceHolder.Inner2Service[0], serviceHolder.Inner2Service[1]);

            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.Inner1Service[0]);
            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.Inner2Service[0]);

            await hostHolder.StopAsync();
            await hostHolder.DisposeAsync();
        }

        private async Task<string> SendTestRequestAsync(string address)
        {
            var message = await new HttpClient().GetAsync($"{address}/test");
            return await message.Content.ReadAsStringAsync();
        }
    }
}
#endif
