using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrintCompany.Data;
using PrintCompany.Mappings;
using Rotativa.AspNetCore;

namespace PrintCompany
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        //    Configuration = builder.Build();
        //}

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<PrintCompanyDbContext>(options => options.UseSqlServer(
            //    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<PrintCompanyDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("SmarterAspConnection")));


            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    //.AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<PrintCompanyDbContext>();

            services.AddMvc(config =>
            {              
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
              .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
             .AddRazorPagesOptions(options =>
            {
                options.AllowAreas = true;
                options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                //options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                options.AccessDeniedPath = $"/Identity/Account/Login";
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Types}/{action=Index}/{id?}");

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Orders}/{action=Index}/{id?}");

                routes.MapRoute(
                    //name: "Default",
                    //url: "{controller}/{action}/{id?}",
                    //defaults: new { area = "", controller = "Home", action = "Index", id = UrlParameter.Optional });
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { area = "", controller = "Orders", action = "Index" });
            });

            // var webRootPath = env.WebRootPath;
            // call rotativa conf passing env to get web root path
            RotativaConfiguration.Setup(env);

        }
    }
}
