#region Using ...
using Framework.Common.Interfaces;
using Framework.Core.IRepositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
#endregion

/*


 */
namespace EGService.Core.IRepositories.Base
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TPrimeryKey"></typeparam>
	public interface IBaseServiceRepository<TEntity, TPrimeryKey> : IBaseFrameworkRepository<TEntity, TPrimeryKey>
		where TEntity : class, IEntityIdentity<TPrimeryKey>
	{

	}
}
