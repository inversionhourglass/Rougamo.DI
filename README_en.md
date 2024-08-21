### Rougamo.DI

`Rougamo.DI` provides a set of IoC/DI extensions for [Rougamo](https://github.com/inversionhourglass/Rougamo) that enhance the IoC/DI interaction experience when using Rougamo.

## Available Extensions

| Package Name                                                                                                        | Description                                                                                                               |
|:-------------------------------------------------------------------------------------------------------------------:|:--------------------------------------------------------------------------------------------------------------------------|
| [Rougamo.Extensions.DependencyInjection.AspNetCore](#rougamoextensionsdependencyinjectionaspnetcore)                | Uses the official `DependencyInjection` and integrates with `HttpContext` to return the correct scoped `IServiceProvider` |
| [Rougamo.Extensions.DependencyInjection.GenericHost](#rougamoextensionsdependencyinjectiongenerichost)              | Uses the official `DependencyInjection`, suitable for non-AspNetCore Generic Host projects                                |
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
    - `Rougamo.Extensions.DependencyInjection.AspNetCore`
    - `Rougamo.Extensions.DependencyInjection.GenericHost`
3. For Autofac extension packages, the major version matches the corresponding official `Autofac` package:
    - `Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore`
    - `Rougamo.Extensions.DependencyInjection.Autofac`

## Rougamo.Extensions.DependencyInjection.AspNetCore

### Quick Start

```csharp
// Register Rougamo (Note: Rougamo does not require registration if you do not need IoC/DI features)
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
    // ... other setup steps
    builder.Services.AddRougamoAspNetCore();
    // ... other setup steps
}

// Accessing IServiceProvider in an aspect
public class TestAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        // Use the extension method GetServiceProvider to obtain the IServiceProvider instance
        var services = context.GetServiceProvider();

        // Utilize IServiceProvider
        var xxx = services.GetService<IXxx>();
    }
}
```

### Non-HttpContext Scope

By default, the extension method `GetServiceProvider` on `MethodContext` only attempts to retrieve the `IServiceProvider` for the current `HttpContext` scope. If there is no `HttpContext`, it will return the root `IServiceProvider`. This design assumes that in AspNetCore projects, scopes are typically not manually created. If you have a scenario where you need to manually create a scope, follow the steps below:

```csharp
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
    // ... other setup steps
    builder.Services.AddNestableHttpContextScopeAccessor();  // Additional registration step
    builder.Services.AddRougamoAspNetCore();
    // ... other setup steps
}

public class Cls(IServiceProvider services)
{
    public void M()
    {
        // Use the extension method CreateResolvableScope to create a scope.
        // If you use CreateScope, that scope will not be accessible in aspect types.
        using var scope = services.CreateResolvableScope();
    }
}
```

## Rougamo.Extensions.DependencyInjection.GenericHost

```csharp
// Register Rougamo (Note: Rougamo does not require registration if you do not need IoC/DI features)
public static void Main(string[] args)
{
    var builder = Host.CreateDefaultBuilder();
    // ... other setup steps
    builder.ConfigureServices(services => services.AddRougamoGenericHost());
    // ... other setup steps
}

// Accessing IServiceProvider in an aspect
public class TestAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        // Use the extension method GetServiceProvider to obtain the IServiceProvider instance
        var services = context.GetServiceProvider();

        // Utilize IServiceProvider
        var xxx = services.GetService<IXxx>();
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

### Non-HttpContext Scope

By default, the `GetAutofacCurrentScope` extension method on `MethodContext` only attempts to retrieve the `ILifetimeScope` for the current `HttpContext`. If there is no `HttpContext`, it will return the root `IServiceProvider`. This design assumes that in AspNetCore projects, scopes are typically not manually created. If you have a scenario where you need to manually create a scope, follow the steps below:

```csharp
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterAutofacNestableHttpContextScopeAccessor(); // Additional registration step
                builder.RegisterRougamoAspNetCore();
            });
    
    // Registering IHttpContextAccessor is also required
    builder.Services.AddHttpContextAccessor();
}

public class Cls(IServiceProvider services)
{
    public void M()
    {
        // Use the extension method BeginResolvableLifetimeScope to create a scope.
        // If you use BeginLifetimeScope, that scope will not be accessible in aspect types.
        using var scope = services.BeginResolvableLifetimeScope();
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
