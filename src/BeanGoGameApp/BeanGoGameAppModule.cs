using AeFinder.Sdk.Processor;
using BeanGoGameApp.GraphQL;
using BeanGoGameApp.Processors;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace BeanGoGameApp;

public class BeanGoGameAppModule: AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<BeanGoGameAppModule>(); });
        context.Services.AddSingleton<ISchema, AeIndexerSchema>();
        
        // Add your LogEventProcessor implementation.
        //context.Services.AddSingleton<ILogEventProcessor, MyLogEventProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, BingoedProcessor>();
        context.Services.AddSingleton<ILogEventProcessor, PlayedProcessor>();
    }
}