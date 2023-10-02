using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //inMemory geliştirirken bu kodları kullanıcaz. sql server a geçince de o kodları yazıcaz bunları comment e alıcaz.
            services.AddDbContext<OrderContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"),
                ServiceLifetime.Singleton,
                ServiceLifetime.Singleton
            );

            //Add Repositories.

            return services;
        }
    }
}
