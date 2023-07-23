#region Using ...
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Common;
using EGService.Core.IRepositories;
using EGService.DataAccess.Contexts;
using EGService.Entity.Entities;
using Microsoft.EntityFrameworkCore;
#endregion

/*


*/
namespace EGService.DataAccess.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersRepositoryAsync : Base.BaseServiceRepositoryAsync<User, long>, IUsersRepositoryAsync
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance from type
        /// UsersRepositoryAsync.
        /// </summary>
        /// <param name="context"></param>
        public UsersRepositoryAsync(EGServiceContext context, ICurrentUserService currentUserService)
            : base(context, currentUserService)
        {

        }
        #endregion


        public User Login(string userName)
        {
            return this.Entities.AsQueryable().FirstOrDefault(x => x.Username == userName &&x.IsDeleted!=true&&x.IsActive==true);
        }

    }
}
