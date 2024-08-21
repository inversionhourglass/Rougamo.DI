### Rougamo.DI

`Rougamo.DI` provides a set of IoC/DI extensions for [Rougamo](https://github.com/inversionhourglass/Rougamo) that enhance the IoC/DI interaction experience when using Rougamo.

## Available Extensions

| Package                                                                                                | Purpose                                                                                                                 |
|:------------------------------------------------------------------------------------------------------:|:------------------------------------------------------------------------------------------------------------------------|
| [Rougamo.Extensions.DependencyInjection.AspNetCore](#rougamoextensionsdependencyinjectionaspnetcore)   | Uses the official `DependencyInjection` with the current `HttpContext` to return the correct scoped `IServiceProvider`. |
| [Rougamo.Extensions.DependencyInjection.GenericHost](#rougamoextensionsdependencyinjectiongenerichost) | Uses the official `DependencyInjection`, designed for non-AspNetCore Generic Host projects.                             |

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
