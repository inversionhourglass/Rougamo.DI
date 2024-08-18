using System.Threading.Tasks;

namespace GenericHost
{
    public class Locker
    {
        private readonly TaskCompletionSource<bool> _lock = new();

        public void Set()
        {
            _lock.SetResult(true);
        }

        public async Task WaitForExecuteAsync()
        {
            await _lock.Task;
        }
    }
}
