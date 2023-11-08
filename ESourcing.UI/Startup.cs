using ESourcing.Core.Entities;
using ESourcing.Core.Repositories;
using ESourcing.Core.Repositories.Base;
using ESourcing.Infrastructure.Data;
using ESourcing.Infrastructure.Repository;
using ESourcing.Infrastructure.Repository.Base;
using ESourcing.UI.Clients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ESourcing.UI
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
            services.AddDbContext<WebAppContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

            }).AddDefaultTokenProviders().AddEntityFrameworkStores<WebAppContext>();


            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddMvc();
            services.AddRazorPages();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));


            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.Cookie.Name = "My Cookie";
            //        options.LoginPath = "Home/Login";
            //        options.LogoutPath = "Home/Logout";
            //        options.ExpireTimeSpan = TimeSpan.FromDays(3);
            //        options.SlidingExpiration = false;
            //    });

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}).AddCookie(options =>
            //    {
            //        options.LoginPath = "/Home/Login";
            //        //options.AccessDeniedPath = "/Account/AccessDenied";
            //        options.LogoutPath = $"/Home/Logout";

            //    });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Home/Login";
                options.LogoutPath = $"/Home/Logout";
            });

            services.AddHttpClient();
            services.AddHttpClient<ProductClient>();
            services.AddHttpClient<AuctionClient>();
            services.AddHttpClient<BidClient>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
