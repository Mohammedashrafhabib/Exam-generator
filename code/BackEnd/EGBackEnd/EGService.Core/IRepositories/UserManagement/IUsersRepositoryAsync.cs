#region Using ...
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGService.Entity.Entities;
#endregion

/*


*/
namespace EGService.Core.IRepositories
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUsersRepositoryAsync : Base.IBaseServiceRepositoryAsync<User, long>
    {
        User Login(string userName);
    }
}
