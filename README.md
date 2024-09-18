# Rougamo.DI

中文 | [English](README_en.md)

`Rougamo.DI`提供了一系列 Rougamo(https://github.com/inversionhourglass/Rougamo) 的IoC/DI扩展，使你在使用 [Rougamo](https://github.com/inversionhourglass/Rougamo) 时获得更好的IoC/DI交互体验。

## 当前扩展一览

|                                                  包名                                                               |                                          用途                                         |
|:-------------------------------------------------------------------------------------------------------------------:|:-------------------------------------------------------------------------------------|
| [Rougamo.Extensions.DependencyInjection.Microsoft](#rougamoextensionsdependencyinjectionmicrosoft)                  | 使用官方`DependencyInjection`，结合当前`HttpContext`，返回正确scope的`IServiceProvider` |
| [Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore](#rougamoextensionsdependencyinjectionautofacaspnetcore) | 使用`Autofac`，结合当前`HttpContext`，返回正确scope的`ILifetimeScope`                   |
| [Rougamo.Extensions.DependencyInjection.Autofac](#rougamoextensionsdependencyinjectionautofac)                      | 使用`Autofac`，适用于非AspNetCore项目                                                  |
| Rougamo.Extensions.DependencyInjection.Abstractions                                                                 | 基础抽象包                                                                  |
| Rougamo.Extensions.DependencyInjection.AspNetCore.Abstractions                                                      | AspNetCore基础抽象包                                                        |

## 版本号说明

**所有版本号格式都采用语义版本号（SemVer）**

1. 两个基础抽象包的版本从`1.0.0`开始增加
    - `Rougamo.Extensions.DependencyInjection.Abstractions`
    - `Rougamo.Extensions.DependencyInjection.AspNetCore.Abstractions`
2. 微软官方DI扩展包，主版本号与官方包（`Microsoft.Extensions.*`）相同
    - `Rougamo.Extensions.DependencyInjection.Microsoft`
3. Autofac扩展包，主版本号与官方包（`Autofac`）相同
    - `Rougamo.Extensions.DependencyInjection.Autofac.AspNetCore`
    - `Rougamo.Extensions.DependencyInjection.Autofac`

## Rougamo.Extensions.DependencyInjection.Microsoft

### 快速开始

**`Rougamo.Extensions.DependencyInjection.Microsoft`依赖于`DependencyInjection.StaticAccessor`，启动项目初始化部分通过`DependencyInjection.StaticAccessor`完成，不同类型项目的初始化方式请跳转[`DependencyInjection.StaticAccessor`](https://github.com/inversionhourglass/DependencyInjection.StaticAccessor)查看**

启动项目引用`DependencyInjection.StaticAccessor.Hosting`
> dotnet add package DependencyInjection.StaticAccessor.Hosting

非启动项目引用`Rougamo.Extensions.DependencyInjection.Microsoft`
> dotnet add package Rougamo.Extensions.DependencyInjection.Microsoft

```csharp
// 注册Rougamo（注：如果你不使用IoC/DI功能，Rougamo默认是不需要注册操作的）
public static void Main(string[] args)
{
    // 1. 初始化。这里用通用主机进行演示，其他类型项目请查看DependencyInjection.StaticAccessor项目的readme
    var builder = Host.CreateDefaultBuilder();

    builder.UsePinnedScopeServiceProvider(); // 仅此一步完成初始化

    var host = builder.Build();

    host.Run();
}

// 在切面类型中获取IServiceProvider实例并使用
public class TestAttribute : MoAttribute
{
    public override void OnEntry(MethodContext context)
    {
        var xxx = context.GetService<IXxx>();
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
