using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SharedLib;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.UsePinnedScopeServiceProvider();

builder.Services.AddScoped<TestService>();

await builder.Build().RunAsync();
