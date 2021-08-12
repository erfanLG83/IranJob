using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using IranJob.Persistence;
using IranJob.Services;
using IranJob.Services.Api;
using IranJob.Services.Contract;
using IranJob.Services.Implementation;
using IranJob.Services.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

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
            services.AddScoped<IFileWorker, FileWorker>(provider =>
            {
                var root = provider.GetService<IHostEnvironment>().ContentRootPath+"/wwwroot" + "/files/";
                return new FileWorker(root);
            });
            services.AddScoped<IranJobDbContext>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "api document",
                    Version = "v1",
                    Description = "document of iranjob api"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //options.IncludeXmlComments(xmlPath);
            });
            //services.AddDbContext<IranJobDbContext>(options =>
            //{
            //    options.UseSqlServer("Server=.;Database=IranJobDB;Trusted_Connection=True;");
            //});
            // services.AddSession();
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                if (!context.Request.Path.StartsWithSegments("/api") &&
                    !context.Request.Path.StartsWithSegments("/swagger")&&
                    !context.Request.Path.StartsWithSegments("/files") &&
                    !context.Request.Path.StartsWithSegments("/hangfire"))
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
                    //appBuilder.UseExceptionHandler(new ExceptionHandlerOptions
                    //{
                    //    ExceptionHandler = (async context =>
                    //    {
                    //        context.Response.StatusCode = 500;
                    //        context.Response.Redirect("/api/jobs");
                    //        await Task.CompletedTask;
                    //    })
                    //    //ExceptionHandler = context =>
                    //    //{
                    //    //    context.Response.Redirect("/");
                    //    //};
                    //});
                    if (Configuration["ShowExceptions"] =="true")
                        appBuilder.UseDeveloperExceptionPage();
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

            app.UseHangfireDashboard();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"),
                appBuilder =>
                {
                    appBuilder.Use(async (context, next) =>
                    {
                        if (context.Response.StatusCode == 404)
                        {
                            await context.Response.WriteAsync(
                                JsonConvert.SerializeObject(new ApiResult(false,ApiResultStatusCode.NotFound))
                                );
                        }
                        await next();
                    });
                });
        }
    }
}
