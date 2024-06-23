using PMS.Persistence.Contexts;
using PMS.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PMS.Application.Interfaces.Repositories;

namespace PMS.Persistence;

public static class ServiceCollectionExtensions
{
    public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddRepositories();
    }

    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString)
        );

        services.AddIdentityCore<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services
            .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
            .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddTransient<IProductRepository, ProductRepository>()
            .AddTransient<ICategoryRepository, CategoryRepository>();
    }
}