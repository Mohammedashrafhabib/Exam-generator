#region Using ...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Common.Enums;
using Framework.Core.Common;
using Framework.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EGService.Common.Enums;
using EGService.Core.IServices;
using EGService.Core.Models.ViewModels;
using EGService.WebAPI.Auth;
#endregion

/*


*/
namespace EGService.WebAPI.Controllers.APIControllers
{
    /// <summary>
    /// Providing an API controller that holds 
    /// end points to manage operations over 
    /// type UsersController.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Data Members
        private readonly IUsersService _UsersService;
        #endregion

        #region Constructors
        public UsersController(
            IUsersService UsersService
            )
        {
            this._UsersService = UsersService;
        }
        #endregion

        #region Actions

        #region Basic Function

      
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("get/{id}")]
        [HttpGet]
        [JwtAuthentication()]
        public async Task<UserViewModel> GetAsync(long id)
        {
            var result = await this._UsersService.GetAsync(id);
            return result;
        }


        /// <summary>
        /// Adds a new User to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("add")]
        [HttpPost]
        public async Task<UserViewModel> AddAsync(UserViewModel model)
        {
            var result = await this._UsersService.AddAsync(model);
            return result;
        }

        /// <summary>
        /// updates User to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        [JwtAuthentication()]
        public async Task<UserViewModel> UpdateAsync(UserViewModel model)
        {
            var result = await this._UsersService.UpdateAsync(model);
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete/{id}")]
        [HttpPost]
        [JwtAuthentication()]
        public async Task DeleteAsync(long id)
        {
            await this._UsersService.DeleteAsync(id);
        }

        #endregion

       

       

        #region Auth
        [AllowAnonymous]
        [Route("user-login")]
        [HttpPost]
        public IActionResult LoginInternal(LoginViewModel model)
        {
            UserLoggedInViewModel user = this._UsersService.Login(model);
            if (user != null)
            {

                return Ok(user);
            }
            else
            {
                return BadRequest("0001");
            }
        }


        [Route("change-password")]
        [HttpPost]
        [JwtAuthentication()]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            await this._UsersService.ChangePasswordAsync(model);
            return Ok();
        }

        [Route("reset-password/{userId}")]
        [HttpPost]
        public async Task<IActionResult> ResetPasswordAsync(long userId)
        {
            await this._UsersService.ResetPasswordAsync(userId);
            return Ok();
        }

        [Route("GetCurrentUser")]
        [HttpGet]
        [JwtAuthentication()]
        public async Task<CurrentUserViewModel> GetCurrentUser()
        {
            var result = await _UsersService.GetCurrentUser();

            return result;
        }

       

        [AllowAnonymous]
        [Route("ForgotPassword")]
        [HttpPost]
        public async Task<bool> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            return await _UsersService.ForgotPassword(forgotPasswordModel);
        }


        //[AllowAnonymous]
        //[Route("login")]
        //[HttpPost]
        //public IActionResult Login(MobileLoginViewModel model)
        //{
        //    MobileUserLoggedInViewModel user = this._UsersService.MobileLogin(model);
        //    if (user != null)
        //    {

        //        return Ok(user);
        //    }
        //    else
        //    {
        //        return BadRequest("0001");
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("details/{id}")]
        [HttpGet]
        [JwtAuthentication()]
        public async Task<UserDetailViewModel> GetDetails(long id)
        {
            var result = await this._UsersService.GetDetailsAsync(id);
            return result;
        }

      

        #endregion

        #endregion
    }
}
