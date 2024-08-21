using System;

namespace RougamoDefLib
{
    public interface IScopeProvider
    {
        IDisposable CreateScope();
    }
}
