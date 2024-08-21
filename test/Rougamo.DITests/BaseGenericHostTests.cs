#if !NET5_0
using GenericHost;
using RougamoDefLib;
using System.Threading.Tasks;
using Xunit;

namespace Rougamo.DITests
{
    public abstract class BaseGenericHostTests<TMain> where TMain : BaseMain, new()
    {
        [Fact]
        public async Task NormalTest()
        {
            var serviceHolder = new ServiceHolder();
            var main = new TMain();
            var hostHolder = main.Execute(serviceHolder);
            await hostHolder.WaitForExecuteAsync();

            Assert.NotNull(serviceHolder.OuterService[0]);
            Assert.Equal(serviceHolder.OuterService[0], serviceHolder.OuterService[1]);

            Assert.NotNull(serviceHolder.Inner1Service[0]);
            Assert.Equal(serviceHolder.Inner1Service[0], serviceHolder.Inner1Service[1]);

            Assert.NotNull(serviceHolder.Inner2Service[0]);
            Assert.Equal(serviceHolder.Inner2Service[0], serviceHolder.Inner2Service[1]);

            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.Inner1Service[0]);
            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.Inner2Service[0]);

            await hostHolder.DisposeAsync();
        }

        [Fact]
        public async Task WithoutRougamoTest()
        {
            var serviceHolder = new ServiceHolder();
            var main = new TMain();
            var hostHolder = main.ExecuteWithoutRougamo(serviceHolder);
            await hostHolder.WaitForExecuteAsync();

            Assert.Null(serviceHolder.OuterService[0]);
            Assert.Equal(serviceHolder.OuterService[0], serviceHolder.OuterService[1]);

            Assert.Null(serviceHolder.Inner1Service[0]);
            Assert.Equal(serviceHolder.Inner1Service[0], serviceHolder.Inner1Service[1]);

            Assert.Null(serviceHolder.Inner2Service[0]);
            Assert.Equal(serviceHolder.Inner2Service[0], serviceHolder.Inner2Service[1]);

            Assert.Equal(serviceHolder.OuterService[0], serviceHolder.Inner1Service[0]);
            Assert.Equal(serviceHolder.OuterService[0], serviceHolder.Inner2Service[0]);

            await hostHolder.DisposeAsync();
        }

        [Fact]
        public async Task TransientTest()
        {
            var serviceHolder = new ServiceHolder();
            var main = new TMain();
            var hostHolder = main.ExecuteTransient(serviceHolder);
            await hostHolder.WaitForExecuteAsync();

            Assert.NotNull(serviceHolder.OuterService[0]);
            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.OuterService[1]);

            Assert.NotNull(serviceHolder.Inner1Service[0]);
            Assert.NotEqual(serviceHolder.Inner1Service[0], serviceHolder.Inner1Service[1]);

            Assert.NotNull(serviceHolder.Inner2Service[0]);
            Assert.NotEqual(serviceHolder.Inner2Service[0], serviceHolder.Inner2Service[1]);

            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.Inner1Service[0]);
            Assert.NotEqual(serviceHolder.OuterService[0], serviceHolder.Inner2Service[0]);

            await hostHolder.DisposeAsync();
        }
    }
}
#endif
