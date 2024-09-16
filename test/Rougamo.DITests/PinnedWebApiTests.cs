#if NETCOREAPP3_1_OR_GREATER
using System.Threading.Tasks;
using WebApiHost;
using Xunit;

namespace Rougamo.DITests
{
    public class PinnedWebApiTests : BaseWebApiTests<PinnedMain>
    {
        [Fact]
        public override Task DisableNestableScopeTest()
        {
            // pinned scope cannot disable nested scope.
            return Task.CompletedTask;
        }
    }
}
#endif
