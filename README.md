# Rougamo.DI

`Rougamo.DI`提供了一系列 Rougamo(https://github.com/inversionhourglass/Rougamo) 的IoC/DI扩展，使你在使用 [Rougamo](https://github.com/inversionhourglass/Rougamo) 时获得更好的IoC/DI交互体验。

## 当前扩展一览

|                                                  包名                                                               |                                          用途                                         |
|:-------------------------------------------------------------------------------------------------------------------:|:-------------------------------------------------------------------------------------|
| [Rougamo.Extensions.DependencyInjection.AspNetCore](#rougamoextensionsdependencyinjectionaspnetcore)                | 使用官方`DependencyInjection`，结合当前`HttpContext`，返回正确scope的`IServiceProvider` |
| [Rougamo.Extensions.DependencyInjection.GenericHost](#rougamoextensionsdependencyinjectiongenerichost)              | 使用官方`DependencyInjection`，适用于非AspNetCore的Generic Host项目                     |
| [Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore](#rougamoextensionsdependencyinjectionautofacaspnetcore) | 使用`Autofac`，结合当前`HttpContext`，返回正确scope的`ILifetimeScope`                   |
| [Rougamo.Extensions.DependencyInjection.Autofac](#rougamoextensionsdependencyinjectionautofac)                      | 使用`Autofac`，适用于非AspNetCore项目                                                  |

## Rougamo.Extensions.DependencyInjection.AspNetCore

### 快速开始

```csharp
// 注册Rougamo（注：如果你不使用IoC/DI功能，Rougamo默认是不需要注册操作的）
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
    // ...省略其他步骤
    builder.Services.AddRougamoAspNetCore();
    // ...省略其他步骤
}

// 在切面类型中获取IServiceProvider实例并使用
public class TestAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        // 使用扩展方法GetServiceProvider获取IServiceProvider实例
        var services = context.GetServiceProvider();

        // 使用IServiceProvider
        var xxx = services.GetService<IXxx>();
    }
}
```

### 非HttpContext Scope

默认情况下通过`MethodContext`的扩展方法`GetServiceProvider`只会尝试获取当前`HttpContext`对应scope的`IServiceProvider`，如果当前没有`HttpContext`，那么就会返回根`IServiceProvider`。这样的设计是因为一般而言我们不会在AspNetCore的项目中手动创建一个scope，如果你确实有手动创建scope的场景需求，请按如下的方式操作：

```csharp
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
    // ...省略其他步骤
    builder.Services.AddNestableHttpContextScopeAccessor();  // 额外的注册步骤
    builder.Services.AddRougamoAspNetCore();
    // ...省略其他步骤
}

public class Cls(IServiceProvider services)
{
    public void M()
    {
        // 调用扩展方法CreateResolvableScope创建scope，这里如果使用CreateScope来创建，那么这个scope将无法在切面类型中获取到
        using var scope = services.CreateResolvableScope();
    }
}
```

## Rougamo.Extensions.DependencyInjection.GenericHost

```csharp
// 注册Rougamo（注：如果你不使用IoC/DI功能，Rougamo默认是不需要注册操作的）
public static void Main(string[] args)
{
    var builder = Host.CreateDefaultBuilder();
    // ...省略其他步骤
    builder.ConfigureServices(services => services.AddRougamoGenericHost());
    // ...省略其他步骤
}

// 在切面类型中获取IServiceProvider实例并使用
public class TestAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        // 使用扩展方法GetServiceProvider获取IServiceProvider实例
        var services = context.GetServiceProvider();

        // 使用IServiceProvider
        var xxx = services.GetService<IXxx>();
    }
}
```

## Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore

### 快速开始

```csharp
// 注册Rougamo（注：如果你不使用IoC/DI功能，Rougamo默认是不需要注册操作的）
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterRougamoAspNetCore();
            });
    
    // 注册IHttpContextAccessor也是必须的
    builder.Services.AddHttpContextAccessor();
}

// 在切面类型中获取ILifetimeScope实例并使用
public class TestAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        // 使用扩展方法GetAutofacCurrentScope获取ILifetimeScope实例
        var scope = context.GetAutofacCurrentScope();

        // 使用ILifetimeScope
        var xxx = scope.Resolve<IXxx>();
    }
}
```

### 非HttpContext Scope

默认情况下通过`MethodContext`的扩展方法`GetAutofacCurrentScope`只会尝试获取当前`HttpContext`对应的`ILifetimeScope`，如果当前没有`HttpContext`，那么就会返回根`IServiceProvider`。这样的设计是因为一般而言我们不会在AspNetCore的项目中手动创建一个scope，如果你确实有手动创建scope的场景需求，请按如下的方式操作：

```csharp
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterAutofacNestableHttpContextScopeAccessor(); // 额外的注册步骤
                builder.RegisterRougamoAspNetCore();
            });
    
    // 注册IHttpContextAccessor也是必须的
    builder.Services.AddHttpContextAccessor();
}

public class Cls(IServiceProvider services)
{
    public void M()
    {
        // 调用扩展方法BeginResolvableLifetimeScope创建scope，这里如果使用BeginLifetimeScope来创建，那么这个scope将无法在切面类型中获取到
        using var scope = services.BeginResolvableLifetimeScope();
    }
}
```

## Rougamo.Extensions.DependencyInjection.Autofac

```csharp
// 注册Rougamo（注：如果你不使用IoC/DI功能，Rougamo默认是不需要注册操作的）
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

// 在切面类型中获取IServiceProvider实例并使用
public class TestAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        // 使用扩展方法GetAutofacCurrentScope获取ILifetimeScope实例
        var scope = context.GetAutofacCurrentScope();

        // 使用ILifetimeScope
        var xxx = scope.Resolve<IXxx>();
    }
}
```
