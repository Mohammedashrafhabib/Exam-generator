#region Using ...
using Framework.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EGService.Core.IRepositories;
using EGService.DataAccess.Contexts;
#endregion

/*


 */
namespace EGService.DataAccess.Repositories
{
	/// <summary>
	/// 
	/// </summary>
	public class UnitOfWorkAsync : IUnitOfWorkAsync
	{
		#region Data Members
		private EGServiceContext _context;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance from type 
		/// UnitOfWorkAsync.
		/// </summary>
		/// <param name="context"></param>
		public UnitOfWorkAsync(EGServiceContext context)
		{
			this._context = context;
		}
		#endregion

		#region IUnitOfWork	
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<int> CommitAsync()
		{
			var result = await this._context.SaveChangesAsync();
			return result;
		}
		#endregion
	}
}
