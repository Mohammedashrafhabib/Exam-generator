#region Using ...
using Framework.Core.Common;
using Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using EGService.Core.IServices.Base;
using EGService.Core.Models.ViewModels;
using EGService.Common.Enums;
#endregion

/*


*/
namespace EGService.Core.IServices
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUsersService : IBaseService
    {
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task ValidateModelAsync(UserViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelCollection"></param>
        /// <returns></returns>
        Task ValidateModelAsync(IEnumerable<UserViewModel> modelCollection);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>

     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserViewModel> GetAsync(long id);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<UserViewModel> AddAsync(UserViewModel model);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<UserViewModel> UpdateAsync(UserViewModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(long id);


        UserLoggedInViewModel Login(LoginViewModel model);

        Task ChangePasswordAsync(ChangePasswordViewModel model);

        Task ResetPasswordAsync(long userId);

        Task<CurrentUserViewModel> GetCurrentUser();

        Task<bool> ForgotPassword(ForgotPasswordModel forgotPasswordModel);
        Task<UserDetailViewModel> GetDetailsAsync(long id);


        #endregion
    }
}
