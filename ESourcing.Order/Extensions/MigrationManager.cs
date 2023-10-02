using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.Infrastructure.Data;
using System;

namespace ESourcing.Order.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabases(this IHost host)  //program.cs icerisinde cağırabilmek icin IHost tipinde nesneye ihtiyacımız var.
        {
            using(var scope = host.Services.CreateScope())
            {
                try
                {
                    var orderContext = scope.ServiceProvider.GetRequiredService<OrderContext>();

                    if(orderContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory") //inMemory calısmıyorsak migrate et.
                    {
                        orderContext.Database.Migrate();  //codefirst yazdığımız icin yaptığımız değişiklikleri vs sql server a migrate edebilmemiz icin.
                    }

                    OrderContextSeed.SeedAsync(orderContext).Wait(); //methodu senktonlaştırdık.

                }
                catch (Exception ex)
                {
                    //herhangi hatada log atma işlemi yapılabilir burada.
                    throw;
                }
            }

            return host;
        }
    }
}
