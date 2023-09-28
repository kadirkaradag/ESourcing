using ESourcing.Sourcing.Data;
using ESourcing.Sourcing.Data.Interfaces;
using ESourcing.Sourcing.Repositories;
using ESourcing.Sourcing.Repositories.Interfaces;
using ESourcing.Sourcing.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Sourcing
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.Configure<SourcingDatabaseSettings>(Configuration.GetSection(nameof(SourcingDatabaseSettings)));
            //Configure un GetSection methodu ile git appsettings.json dan nameof yani ad� SourcingDatabaseSettings olan sectionunu al diyoruz.

            services.AddSingleton<ISourcingDatabaseSettings>(sp=>sp.GetRequiredService<IOptions<SourcingDatabaseSettings>>().Value);
            //ISourcingDatabaseSettings istedi�im zaman service provider ile git appsettings.jsondaki SourcingDatabaseSettings classdaki de�erlere kars�l�k gelen de�erlerle bi obje olustur diyoruz. ConnectionString mesela

            services.AddTransient<ISourcingContext, SourcingContext>();

            services.AddTransient<IAuctionRepository, AuctionRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ESourcing.Sourcing", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ESourcing.Sourcing v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
