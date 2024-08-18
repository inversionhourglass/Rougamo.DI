namespace RougamoDefLib
{
    public class TestService : ITestService
    {
        public override string ToString() => $"Hash({GetHashCode()})";
    }
}
