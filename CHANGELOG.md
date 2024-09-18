- 新增`Rougamo.Extensions.DependencyInjection.Microsoft`，删除`Rougamo.Extensions.DependencyInjection.GenericHost`和`Rougamo.Extensions.DependencyInjection.AspNetCore`
    
    Dependency Injection核心逻辑移动到`DependencyInjection.StaticAccessor`系列包中，`Rougamo.Extensions.DependencyInjection.Microsoft`仅提供肉夹馍`MethodContext`系列扩展方法

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

---

