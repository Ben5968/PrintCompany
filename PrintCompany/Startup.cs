﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            services.AddDbContext<PrintCompanyDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDefaultIdentity<IdentityUser>()
            //        .AddEntityFrameworkStores<PrintCompanyDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
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
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

        }
    }
}
