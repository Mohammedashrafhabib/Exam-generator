#region Using ...
using Framework.Core.Common;
using Framework.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EGService.Business.Common;
using EGService.Business.Services;
using EGService.BusinessLogic.Common;
using EGService.Core.IRepositories;
using EGService.Core.IServices;
using EGService.Core.Common;
using EGService.DataAccess.Contexts;
using EGService.DataAccess.Repositories;
using Python.Runtime;

#endregion

/*


 */
namespace EGService.DependancyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public class ContainerConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            #region Register AutoMapper
            services.AddAutoMapper(typeof(Core.Profile).Assembly);
            #endregion

            services.AddHttpContextAccessor();

            services.AddScoped<ILoggerService, LoggerService>();

            #region Add Db Context
            services.AddDbContext<EGServiceContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionString:EGServiceConnection"], b => b.MigrationsAssembly("EGService.DataAccess"));
            });
            #endregion

            #region Register EGServiceContext
            services.AddScoped<EGServiceContext, EGServiceContext>();

            #endregion

            #region Register Repositories
            services.AddScoped<IUsersRepositoryAsync, UsersRepositoryAsync>();
           

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkAsync, UnitOfWorkAsync>();
           

            #endregion




            #region Register Services
            services.AddScoped<IExamGenrator, ExamGenrator>();
            services.AddScoped<IUsersService, UsersService>();
    
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IJwtService, JwtService>();
          
            services.AddSingleton<IMailNotification, MailNotificationService>();
            #endregion
        }
    }
}
