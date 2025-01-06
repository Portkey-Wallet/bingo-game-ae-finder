using AeFinder.App.TestBase;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace BeanGoGameApp;

[DependsOn(
    typeof(AeFinderAppTestBaseModule),
    typeof(BeanGoGameAppModule))]
public class BeanGoGameAppTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AeFinderAppEntityOptions>(options => { options.AddTypes<BeanGoGameAppModule>(); });
        
        // Add your Processors.
        // context.Services.AddSingleton<MyLogEventProcessor>();
    }
}