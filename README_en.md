### Rougamo.DI

[中文](README.md) | English

`Rougamo.DI` provides a set of IoC/DI extensions for [Rougamo](https://github.com/inversionhourglass/Rougamo) that enhance the IoC/DI interaction experience when using Rougamo.

## Available Extensions

| Package Name                                                                                                        | Description                                                                                                               |
|:-------------------------------------------------------------------------------------------------------------------:|:--------------------------------------------------------------------------------------------------------------------------|
| [Rougamo.Extensions.DependencyInjection.Microsoft](#rougamoextensionsdependencyinjectionmicrosoft)                  | Uses the official `DependencyInjection` and integrates with `HttpContext` to return the correct scoped `IServiceProvider` |
| [Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore](#rougamoextensionsdependencyinjectionautofacaspnetcore) | Uses `Autofac` and integrates with `HttpContext` to return the correct scoped `ILifetimeScope`                            |
| [Rougamo.Extensions.DependencyInjection.Autofac](#rougamoextensionsdependencyinjectionautofac)                      | Uses `Autofac`, suitable for non-AspNetCore projects                                                                      |
| Rougamo.Extensions.DependencyInjection.Abstractions                                                                 | The base abstraction package for all other packages                                                                       |
| Rougamo.Extensions.DependencyInjection.AspNetCore.Abstractions                                                      | The base abstraction package for all AspNetCore-related packages                                                          |

### Versioning Guidelines

**All version numbers follow the Semantic Versioning (SemVer) format**

1. The version numbers of the two foundational abstraction packages start from `1.0.0` and increase incrementally:
    - `Rougamo.Extensions.DependencyInjection.Abstractions`
    - `Rougamo.Extensions.DependencyInjection.AspNetCore.Abstractions`
2. For Microsoft official DI extension packages, the major version matches the corresponding official package (e.g., `Microsoft.Extensions.*`):
    - `Rougamo.Extensions.DependencyInjection.Microsoft`
3. For Autofac extension packages, the major version matches the corresponding official `Autofac` package:
    - `Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore`
    - `Rougamo.Extensions.DependencyInjection.Autofac`

## Rougamo.Extensions.DependencyInjection.Microsoft

### Quick Start

**`Rougamo.Extensions.DependencyInjection.Microsoft` depends on `DependencyInjection.StaticAccessor`. The initialization of the startup project is completed via `DependencyInjection.StaticAccessor`. For different types of project initialization, please refer to [`DependencyInjection.StaticAccessor`](https://github.com/inversionhourglass/DependencyInjection.StaticAccessor).**

For the startup project, reference `DependencyInjection.StaticAccessor.Hosting`:
> dotnet add package DependencyInjection.StaticAccessor.Hosting

For non-startup projects, reference `Rougamo.Extensions.DependencyInjection.Microsoft`:
> dotnet add package Rougamo.Extensions.DependencyInjection.Microsoft

```csharp
// Register Rougamo (Note: If you're not using IoC/DI functionality, Rougamo does not require registration by default)
public static void Main(string[] args)
{
    // 1. Initialization. This example uses a generic host; for other project types, please refer to the readme of the DependencyInjection.StaticAccessor project.
    var builder = Host.CreateDefaultBuilder();

    builder.UsePinnedScopeServiceProvider(); // Initialization completed with this single step

    var host = builder.Build();

    host.Run();
}

// Retrieve and use an IServiceProvider instance in an aspect type
public class TestAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        var xxx = context.GetService<IXxx>();
    }
}
```

## Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore

### Quick Start

```csharp
// Register Rougamo (Note: Rougamo does not require registration if you do not need IoC/DI features)
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterRougamoAspNetCore();
            });
    
    // Registering IHttpContextAccessor is also required
    builder.Services.AddHttpContextAccessor();
}

// Accessing ILifetimeScope in an aspect
public class TestAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        // Use the extension method GetAutofacCurrentScope to obtain the ILifetimeScope instance
        var scope = context.GetAutofacCurrentScope();

        // Utilize ILifetimeScope
        var xxx = scope.Resolve<IXxx>();
    }
}
```

## Rougamo.Extensions.DependencyInjection.Autofac

```csharp
// Register Rougamo (Note: Rougamo does not require registration if you do not need IoC/DI features)
public static void Main(string[] args)
{
    var builder = Host.CreateDefaultBuilder();
    
    builder
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.RegisterRougamo();
        });
}

// Accessing ILifetimeScope in an aspect
public class TestAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        // Use the extension method GetAutofacCurrentScope to obtain the ILifetimeScope instance
        var scope = context.GetAutofacCurrentScope();

        // Utilize ILifetimeScope
        var xxx = scope.Resolve<IXxx>();
    }
}
```

### Usage in Framework Projects

If your project is an older WebForm, WinForm, WPF, or similar project that does not use the `Microsoft.Extensions.*` packages, you can directly call the `RegisterRougamo` extension method when initializing the `ContainerBuilder`.

```csharp
var builder = new ContainerBuilder();
builder.RegisterRougamo();
```
