#region Using ...
using Framework.Common.Interfaces;
using Framework.Core.Common;
using Framework.DataAccess.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using EGService.Core.IRepositories.Base;
using EGService.DataAccess.Contexts;
#endregion

/*


 */
namespace EGService.DataAccess.Repositories.Base
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <typeparam name="TPrimeryKey"></typeparam>
	public class BaseServiceRepositoryAsync<TEntity, TPrimeryKey> : BaseFrameworkRepositoryAsync<TEntity, TPrimeryKey>,
		IBaseServiceRepositoryAsync<TEntity, TPrimeryKey>
		where TEntity : class, IEntityIdentity<TPrimeryKey>
	{
		#region Data Members
		private EGServiceContext _EGContext;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance from type 
		/// BaseServiceRepositoryAsync.
		/// </summary>
		/// <param name="context"></param>
		public BaseServiceRepositoryAsync(EGServiceContext context, ICurrentUserService currentUserService)
			: base(context, currentUserService)
		{
			this.EGContext = context;
		}
		#endregion

		#region Properties
		protected EGServiceContext EGContext
		{
			get { return this._EGContext; }
			private set { this._EGContext = value; }
		}
		#endregion
	}
}
