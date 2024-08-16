using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Rougamo.Extensions.DependencyInjection
{
    /// <summary>
    /// </summary>
    public class ScopeChain : Stack<IServiceScope>
    {
        /// <summary>
        /// </summary>
        public bool TryPop()
        {
#if NETSTANDARD2_0
                if (Count > 0)
                {
                    Pop();
                    return true;
                }
                return false;
#else
            return TryPop(out _);
#endif
        }
    }
}
