namespace RougamoDefLib
{
    public class ServiceHolder
    {
        public ITestService?[] OuterService { get; } = new TestService[2];

        public ITestService?[] Inner1Service { get; } = new TestService[2];

        public ITestService?[] Inner2Service { get; } = new TestService[2];
    }
}
