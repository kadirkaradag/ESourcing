using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Repositories;
using Ordering.Domain.Repositories.Base;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Repositories.Base;

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

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>)); //typeof kullanmanın nedeni best practise olarak kendi içerisinde tip alan interfaceleri lifetime a eklerken type of ile ekliyoruz bu sekilde
            services.AddTransient<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
