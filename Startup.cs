using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace middleware_test1
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


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /*
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            */

            // app.Use(async (context, next) =>
            //     {
            //         Console.WriteLine("A (before)");
            //         await next();
            //         Console.WriteLine("A (after)");
            //     });
            
            // app.Map(
            //     new PathString("/foo"),
            //     a => a.Use(async (context, next) =>
            //     {
            //         Console.WriteLine("B (before)");
            //         await next();
            //         Console.WriteLine("B (after)");
            //     }));
        
            // app.Run(async context =>
            // {
            //     Console.WriteLine("C");
            //     await context.Response.WriteAsync("Hello world");
            // });
            app.Use(async (context, next) =>
            {
                Console.WriteLine("A (before)");
                await next();
                Console.WriteLine("A (after)");
            });
        
            app.UseWhen(
                context => context.Request.Path.StartsWithSegments(new PathString("/foo")),
                a => a.Use(async (context, next) =>
                {
                    Console.WriteLine("B (before)");
                    await next();
                    Console.WriteLine("B (after)");
                }));
        
            app.Run(async context =>
            {
                Console.WriteLine("C");
                await context.Response.WriteAsync("Hello world");
            });
        }
    }
}
