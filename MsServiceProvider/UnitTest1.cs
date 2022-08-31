using Xunit.Abstractions;

namespace MsServiceProvider;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class UnitTest1
{
    private readonly ITestOutputHelper _helper;

    public UnitTest1(ITestOutputHelper helper)
    {
        _helper = helper;
    }

    public interface IMyService
    {
        string DoSomething();
    }

    public class Service1: IMyService
    {
        public string DoSomething()
        {
            return "s1";
        }
    }
    
    public class Service2: IMyService
    {
        public string DoSomething()
        {
            return "s2";
        }
    }
    
    [Fact]
    public void Test1()
    {
        var services = new ServiceCollection();
        services
            .AddSingleton<IMyService, Service1>()
            .AddSingleton<IMyService, Service2>();
        
            
        var provider = services.BuildServiceProvider();
        
        foreach (var myService in provider.GetServices<IMyService>())
        {   
            _helper.WriteLine($"Msg is {myService.DoSomething()}");
        }
    }
}