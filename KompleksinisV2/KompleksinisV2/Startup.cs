﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KompleksinisV2.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using KompleksinisV2.Data;
using Microsoft.AspNetCore.Identity;

namespace KompleksinisV2
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
          //  services.AddDbContext<TestContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
            services.AddDbContext<TestContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //TODO : ADD IDENTITY AUTHENTICATION

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options => 
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login/");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Forbidden/");
                });

            services.AddMvc();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Dashboard/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
