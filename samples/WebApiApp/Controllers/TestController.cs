using Microsoft.AspNetCore.Mvc;
using SharedLib;
using System;

namespace WebApiApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController(TestService testService) : ControllerBase
    {
        [Getter]
        public TestService GetTestService() => throw new NotImplementedException();

        [HttpGet]
        public string Get()
        {
            // should be true here
            var isEqual = testService == GetTestService();

            return $"IsEqual: {isEqual}";
        }
    }
}
