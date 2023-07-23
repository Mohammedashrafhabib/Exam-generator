#region Using ...
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
#endregion

/*


 */
namespace Framework.Core.IRepositories
{
	/// <summary>
	/// 
	/// </summary>
	public interface IUnitOfWork
	{
		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		int Commit();
		#endregion
	}
}
