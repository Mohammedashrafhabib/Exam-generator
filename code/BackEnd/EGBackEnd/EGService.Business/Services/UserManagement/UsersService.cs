#region Using ...
using AutoMapper;
using EGService.Common.Enums;
using EGService.Core;
using EGService.Core.Common;
using EGService.Core.Helper;
using EGService.Core.IRepositories;
using EGService.Core.IServices;
using EGService.Core.Models.ViewModels;
using EGService.Entity.Entities;
using Framework.Common.Enums;
using Framework.Common.Exceptions;
using Framework.Core.Common;
using Framework.Core.IRepositories;
using Framework.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
#endregion

/*


*/
namespace EGService.Business.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersService : Base.BaseService, IUsersService
    {
        #region Data Members
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IUsersRepositoryAsync _UsersRepositoryAsync;
        private readonly ICurrentUserService _currentUserService;
      

        private readonly IJwtService _jwtService;
        private readonly IMailNotification _mailNotification;



        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance from type
        /// UsersService.
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="languageService"></param>
        /// <param name="unitOfWorkAsync"></param>
        /// <param name="ExcelService"></param>
        /// <param name="UsersRepositoryAsync"></param>
        public UsersService(
            IMapper mapper,
            ILoggerService logger,
            IUnitOfWorkAsync unitOfWorkAsync,
            IUsersRepositoryAsync UsersRepositoryAsync,
            ICurrentUserService currentUserService,

            IJwtService jwtService,
            IMailNotification mailNotification
            )
        {
            this._mapper = mapper;
            this._loggerService = logger;
            this._unitOfWorkAsync = unitOfWorkAsync;
            this._UsersRepositoryAsync = UsersRepositoryAsync;
            this._currentUserService = currentUserService;

 
            _jwtService = jwtService;

            this._mailNotification = mailNotification;
        }
        #endregion

        #region Methods
        private User UpdateExistEntityFromModel(User existEntity, User newEntity)
        {
            #region Update Exist Entity Properties
            /*
             * Update properties that need updates here
             * Example:
             * existEntity.<property> = newEntity.<property>;
             */
            existEntity.Username = newEntity.Email;
            existEntity.Password = newEntity.Password;
            existEntity.IsActive = newEntity.IsActive;

            existEntity.FirstName = newEntity.FirstName;
            existEntity.LastName = newEntity.LastName;
            existEntity.Email = newEntity.Email;

            #endregion

            return existEntity;
        }
        #endregion

        #region IUsersService
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task ValidateModelAsync(UserViewModel model)
        {
            await Task.Run(() =>
            {

                var existEntity = this._UsersRepositoryAsync.GetAsync(null).Result.FirstOrDefault(entity =>
                        (entity.Username == model.Username || entity.Email == model.Email) &&
                        entity.Id != model.Id && entity.IsDeleted != true);

                if (existEntity != null)
                    throw new ItemAlreadyExistException((int)ErrorCode.UserNameAlreadyExist);
            });
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelCollection"></param>
        /// <returns></returns>
        public async Task ValidateModelAsync(IEnumerable<UserViewModel> modelCollection)
        {
            await Task.Run(() =>
            {

            });
        }


        /// <summary>
        /// Gets an entity by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserViewModel> GetAsync(long id)
        {
            var include = new string[]
            {
            };
            var query = await this._UsersRepositoryAsync.GetAsync(null);
            query = await this._UsersRepositoryAsync.SetIncludedNavigationsListAsync(query, include);
            var userId = _currentUserService.CurrentUserId;
            var CurrentUser = await _UsersRepositoryAsync.FirstOrDefaultAsync(u => u.Id == userId);
            query = query.Where(entity => entity.Id == id);
           
            var entity = query.FirstOrDefault();
            if (entity == null)
                throw new Framework.Common.Exceptions.InvalidOperationException((int)ErrorCode.InvalidOperationException);
            var model = entity.ToModel(this._mapper);

            return model;
        }

        public async Task<UserDetailViewModel> GetDetailsAsync(long id)
        {

            var include = new string[]
           {
               
           };
            var entity = await this._UsersRepositoryAsync.FirstOrDefaultAsync(x => x.Id == id, include);
            var model = entity.ToDetailModel(this._mapper);
           
           
            return model;
        }



        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserViewModel> AddAsync(UserViewModel model)
        {

            await this.ValidateModelAsync(model);
            var entity = model.ToEntity(this._mapper);
            entity.CreatedByUserId = _currentUserService.CurrentUserId;
            entity.IsActive = true;
            entity.Password = HashPass.HashPassword("EG@123456");
            entity = await this._UsersRepositoryAsync.AddAsync(entity);
            var Password = "\"EG@123456\"";
            var HtmlContent = $@" 
                            <p>Welcome to EG. <br> <br>
                             Kindly use the current password for your account as {Password}. <br> <br>
                             Kind regards,<br>
                             EG
                            </p>";
            await _mailNotification.SendMail(entity.Email, null, null, "Register", HtmlContent);
            #region Commit Changes
            await this._unitOfWorkAsync.CommitAsync();
            #endregion
    
           


            var result = entity.ToModel(this._mapper);
            return result;
        }


        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserViewModel> UpdateAsync(UserViewModel model)
        {
            await this.ValidateModelAsync(model);
            var entity = model.ToEntity(this._mapper);

            #region Select Existing Entity
            var existEntity = await this._UsersRepositoryAsync.FirstOrDefaultAsync(x => x.Id == model.Id);
            #endregion
            entity.Password = existEntity.Password;
            entity.Username = existEntity.Username;

            this.UpdateExistEntityFromModel(existEntity, entity);
          
            existEntity = await this._UsersRepositoryAsync.UpdateAsync(existEntity);


            #region Commit Changes
            await this._unitOfWorkAsync.CommitAsync();
            #endregion


            var result = existEntity.ToModel(this._mapper);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            var userId = _currentUserService.CurrentUserId;
            var CurrentUser = await _UsersRepositoryAsync.FirstOrDefaultAsync(u => u.Id == userId);
           
            await this._UsersRepositoryAsync.DeleteAsync(id);

            #region Commit Changes
            await this._unitOfWorkAsync.CommitAsync();
            #endregion
        }


     

        public UserLoggedInViewModel Login(LoginViewModel model)
        {

            UserLoggedInViewModel viewModel = new UserLoggedInViewModel();
            var user = this._UsersRepositoryAsync.Login(model.UserName);
            if (user != null)
            {

                if (VerifyHashedPassword(user.Password, model.Password))
                {
                    
                   
                    viewModel.Id = user.Id;
                    viewModel.UserName = user.Username;
                    viewModel.token_type = "Bearer";
                    viewModel.issued = DateTime.Now;
                    viewModel.Email = user.Email;
                    viewModel.FirstName = user.FirstName;
                    viewModel.LastName = user.LastName;
                   
                    viewModel.Name =  user.FirstName + user.LastName;
                    


                   

                   

          
                    viewModel.access_token = _jwtService.GenerateJWTToken(user.Id.ToString(), "");

                    #region	 Check if it frst time login
                    viewModel.IsFirstTimeLogin = false;
                    #endregion
                    return viewModel;
                }
                else
                {
                   

                    return null;
                }
            }
            else // login false
            {
              
                return null;
            }

        }

        public async Task ChangePasswordAsync(ChangePasswordViewModel model)
        {
            _loggerService.LogInfo(JsonSerializer.Serialize(model));
            User user = await this._UsersRepositoryAsync.GetAsync(model.UserId);
            string newPasswordHash = HashPass.HashPassword(model.NewPassword);
            if (VerifyHashedPassword(user.Password, model.OldPassword))
            {
                user.Password = newPasswordHash;
                await this._UsersRepositoryAsync.UpdateAsync(user);

                

                await this._unitOfWorkAsync.CommitAsync();
            }
            else
            {
                ///throw new GeneralException((int)ErrorCodeEnum.PasswordIncorrect);
                throw new NotImplementedException("error." + (int)ErrorCode.PasswordIncorrect);
            }
        }

        public async Task ResetPasswordAsync(long userId)
        {
            User user = await this._UsersRepositoryAsync.GetAsync(userId);
            string passwordHash = HashPass.HashPassword("123456");
            user.Password = passwordHash;
            await this._UsersRepositoryAsync.UpdateAsync(user);
            await this._unitOfWorkAsync.CommitAsync();
        }


        #endregion

        #region Helper

        private static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            //return hashedPassword == password;

            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }
        private static bool ByteArraysEqual(byte[] firstHash, byte[] secondHash)
        {
            int _minHashLength = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (int i = 0; i < _minHashLength; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }

        public async Task<CurrentUserViewModel> GetCurrentUser()
        {

            var res = await this._UsersRepositoryAsync.GetAsync(null);
            var currentUser = res.Where(x => x.Id == _currentUserService.CurrentUserId).Select(x => new CurrentUserViewModel()
            {
                Id = x.Id,
                Name = x.FirstName + x.LastName
            }).FirstOrDefault();
            return currentUser;
        }





     

        public async Task<bool> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            var entity = await this._UsersRepositoryAsync.FirstOrDefaultAsync(x => x.Email == forgotPasswordModel.Email);

            if (entity != null)
            {
                var Pass = "EG@";
                Random rand = new Random();
                int number = rand.Next(100000, 1000000); //returns random number between 0-99
                Pass = Pass + number.ToString();
                string passwordHash = HashPass.HashPassword(Pass);
                entity.Password = passwordHash;
                entity = await this._UsersRepositoryAsync.UpdateAsync(entity);
                await this._unitOfWorkAsync.CommitAsync();
               // var Password = "\"{EG@12345}\"";
                var HtmlContent = $@" 
                            <p>Welcome to EG. <br> <br>
                             Kindly use the current password for your account as ' {Pass}' <br> <br>
                             <a href='https://www.egEG.com/' > EG </a>  <br> <br>
                             Kind regards,<br>
                             EG
                            </p>";
                await _mailNotification.SendMail(entity.Email, null, null, "Forgot Password", HtmlContent);
                return true;

            }
            else
                throw new ItemNotFoundException();
        }




        #endregion

        #region private method

        #endregion
    }
}
