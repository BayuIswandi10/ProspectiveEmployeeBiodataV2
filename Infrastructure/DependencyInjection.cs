using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core SQL Server
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBiodataRepository, BiodataRepository>();
        services.AddScoped<IPendidikanRepository, PendidikanRepository>();
        services.AddScoped<IPelatihanRepository, PelatihanRepository>();
        services.AddScoped<IPekerjaanRepository, PekerjaanRepository>();

        return services;
    }
}
