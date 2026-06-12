using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        // Domain layer has no DI registrations itself — only interfaces/entities
        return services;
    }
}
