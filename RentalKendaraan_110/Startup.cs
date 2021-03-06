﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentalKendaraan_110.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentalKendaraan_110.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace RentalKendaraan_110
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<Models.Rental_KendaraanContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddDefaultIdentity<IdentityUser>()
            // .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultUI()
                .AddEntityFrameworkStores<Rental_KendaraanContext>().AddDefaultTokenProviders();

            //
            services.AddAuthorization(options =>
            {
                options.AddPolicy("readonlypolicy",
                    Builder => Builder.RequireRole("Admin", "Manager", "Kasir"));
                options.AddPolicy("writepolicy",
                    Builder => Builder.RequireRole("Admin", "Kasir"));
                options.AddPolicy("editpolicy",
                    Builder => Builder.RequireRole("Admin", "Kasir"));
                options.AddPolicy("deletepolicy",
                    Builder => Builder.RequireRole("Admin", "Kasir"));
            });
            services.AddScoped<Peminjaman>();
            services.AddScoped<Pengembalian>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
