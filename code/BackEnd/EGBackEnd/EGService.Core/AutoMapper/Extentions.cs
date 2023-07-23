#region Using ...
using EGService.Core.Models.ViewModels;

using EGService.Entity.Entities;

#endregion

/*


 */
namespace EGService.Core
{
    /// <summary>
    /// Provides an extention methods that used for mapping.
    /// </summary>
    public static class Extentions
    {
        #region User
        public static User ToEntity(this UserViewModel model, AutoMapper.IMapper mapper)
        {
            var result = mapper.Map<UserViewModel, User>(model);
            return result;
        }
        public static UserViewModel ToModel(this User entity, AutoMapper.IMapper mapper)
        {
            var result = mapper.Map<User, UserViewModel>(entity);
            return result;
        }
        public static UserDetailViewModel ToDetailModel(this User entity, AutoMapper.IMapper mapper)
        {
            var result = mapper.Map<User, UserDetailViewModel>(entity);
            return result;
        }
        public static UserViewViewModel ToViewViewModel(this User entity, AutoMapper.IMapper mapper)
        {
            var result = mapper.Map<User, UserViewViewModel>(entity);
            return result;
        }
     
        #endregion

       
      

      

       

    }
}
