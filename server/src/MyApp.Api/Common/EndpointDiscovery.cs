using System.Reflection;

namespace MyApp.Common;

public static class EndpointDiscovery
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var endpointTypes = typeof(EndpointDiscovery).Assembly
            .GetTypes()
            .Where(type => typeof(IEndpoint).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
            .OrderBy(type => type.Name)
            .ToList();

        foreach (var endpointType in endpointTypes)
        {
            if (Activator.CreateInstance(endpointType) is IEndpoint endpoint)
            {
                endpoint.Map(app);
            }
        }
    }
}
