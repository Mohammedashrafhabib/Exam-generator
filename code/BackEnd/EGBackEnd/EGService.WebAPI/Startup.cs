#region Using ...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Framework.Core.Common;
using Hangfire;
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
using EGService.Business.Services;
using EGService.Core.Common;
using EGService.Core.IRepositories;
using EGService.Core.IServices;
using EGService.DataAccess.Contexts;
using EGService.DataAccess.Repositories;
using EGService.DependancyInjection;
using EGService.WebAPI.Middlewares;
#endregion


// for JWT see: https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api
namespace EGService.WebAPI
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance from type 
        /// Startup.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }
        #endregion

        #region Methods
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(
            #region Register HttpResponseExceptionFilter if you do not use CustomExceptionMiddleware
                    //options =>
                    //options.Filters.Add(new HttpResponseExceptionFilter()) 
            #endregion
                    )
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            #region AddHangfire
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration["ConnectionString:EGServiceConnection"])
            );


            services.AddHangfireServer();
     
            #endregion

            #region AddSignalR
            services.AddSignalR();
            #endregion

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddPolicy("allowall", builder =>
                {
                    //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                    //builder.AllowAnyOrigin()
                    builder.WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            Configuration["CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o)
                                .ToArray()
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
      services.AddCors(setup => setup.AddPolicy("allowall", policy =>
      {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
      }));


      ContainerConfiguration.Configure(services, this.Configuration);
       }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerService logger,
                IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Use Exception Middleware
            app.ConfigureExceptionHandler(logger);
            //app.ConfigureCustomExceptionMiddleware(); 
            #endregion

            EGService.Core.Profile.ApplicationBuilder = app;
          //  app.UseHangfireDashboard();
            var isEnableHangfireMigration = Configuration.GetSection("Hangfire").GetSection("isEnableHangfireMigration").Value;
            if (isEnableHangfireMigration == "True")
            {
                
            }
            app.UseCors("allowall");
       



            app.UseStaticFiles();

            app.UseHttpsRedirection();

            var isEnableSwaagger = Configuration.GetSection("CommonSettings").GetSection("isEnableSwagger").Value;
            if (isEnableSwaagger == "True")
            {
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
             

            });



          

            try
            {
                UpdateDatabase(app);
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<EGServiceContext>())
                {
                    //context.Database.EnsureCreated();
                    context.Database.Migrate();
                }
            }
        }
        #endregion
    }
}
