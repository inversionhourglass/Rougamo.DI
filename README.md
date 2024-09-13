# Rougamo.DI

`Rougamo.DI`提供了一系列 Rougamo(https://github.com/inversionhourglass/Rougamo) 的IoC/DI扩展，使你在使用 [Rougamo](https://github.com/inversionhourglass/Rougamo) 时获得更好的IoC/DI交互体验。

## 当前扩展一览

|                                                  包名                                                               |                                          用途                                         |
|:-------------------------------------------------------------------------------------------------------------------:|:-------------------------------------------------------------------------------------|
| [Rougamo.Extensions.DependencyInjection.AspNetCore](#rougamoextensionsdependencyinjectionaspnetcore)                | 使用官方`DependencyInjection`，结合当前`HttpContext`，返回正确scope的`IServiceProvider` |
| [Rougamo.Extensions.DependencyInjection.GenericHost](#rougamoextensionsdependencyinjectiongenerichost)              | 使用官方`DependencyInjection`，适用于非AspNetCore的Generic Host项目                     |
| [Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore](#rougamoextensionsdependencyinjectionautofacaspnetcore) | 使用`Autofac`，结合当前`HttpContext`，返回正确scope的`ILifetimeScope`                   |
| [Rougamo.Extensions.DependencyInjection.Autofac](#rougamoextensionsdependencyinjectionautofac)                      | 使用`Autofac`，适用于非AspNetCore项目                                                  |
| Rougamo.Extensions.DependencyInjection.Abstractions                                                                 | 所有其他包的基础抽象包                                                                  |
| Rougamo.Extensions.DependencyInjection.AspNetCore.Abstractions                                                      | 所有AspNetCore相关包的基础抽象包                                                        |

## 版本号说明

**所有版本号格式都采用语义版本号（SemVer）**

1. 两个基础抽象包的版本从`1.0.0`开始增加
    - `Rougamo.Extensions.DependencyInjection.Abstractions`
    - `Rougamo.Extensions.DependencyInjection.AspNetCore.Abstractions`
2. 微软官方DI扩展包，主版本号与官方包（`Microsoft.Extensions.*`）相同
    - `Rougamo.Extensions.DependencyInjection.AspNetCore`
    - `Rougamo.Extensions.DependencyInjection.GenericHost`
3. Autofac扩展包，主版本号与官方包（`Autofac`）相同
    - `Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore`
    - `Rougamo.Extensions.DependencyInjection.Autofac`

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

### 在Framework项目中使用

如果你的项目是老的WebForm或WinForm、WPF等项目，没有使用`Microsoft.Extensions.*`系列包，那么在应用初始化创建`ContainerBuilder`时直接调用扩展方法`RegisterRougamo`即可。

```csharp
var builder = new ContainerBuilder();
builder.RegisterRougamo();
```
