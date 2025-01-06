using AeFinder.Sdk;

namespace BeanGoGameApp.GraphQL;

public class AeIndexerSchema : AppSchema<Query>
{
    public AeIndexerSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}