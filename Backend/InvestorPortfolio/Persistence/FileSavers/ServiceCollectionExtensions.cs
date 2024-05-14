using Microsoft.Extensions.DependencyInjection;
using Persistence.Interfaces;

namespace Persistence.FileSavers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileSavers(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAsyncFileSaver<>), typeof(JsonSaver<>)); 
        services.AddScoped(typeof(IFileSaver<>), typeof(XmlSaver<>));
        
        return services; 
    }
}