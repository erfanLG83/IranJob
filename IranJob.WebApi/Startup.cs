using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IranJob.Persistence;
using IranJob.Services;
using IranJob.Services.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace IranJob.WebApi
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
            services.AddServices();
            services.AddIdentityServices();
            services.AddJwtAuthentication();
            services.AddControllers();
            services.AddScoped<IranJobDbContext>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",new OpenApiInfo()
                {
                    Title = "api document",
                    Version = "v1",
                    Description = "document of iranjob api"
                });
            });
            //services.AddDbContext<IranJobDbContext>(options =>
            //{
            //    options.UseSqlServer("Server=.;Database=IranJobDB;Trusted_Connection=True;");
            //});
            // services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                if (!context.Request.Path.StartsWithSegments("/api") &&
                    !context.Request.Path.StartsWithSegments("/swagger"))
                {
                    context.Response.Redirect("/swagger");
                }
                else
                    await next();

            });
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"),
                appBuilder =>
                {
                    appBuilder.UseApiExeption();
                }
            );
            app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api"),
                appBuilder =>
                {
                    if (env.IsDevelopment())
                        appBuilder.UseDeveloperExceptionPage();
                    else
                        appBuilder.UseExceptionHandler();
                }
            );
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
            //app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
