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
	public class BaseServiceRepository<TEntity, TPrimeryKey> : BaseFrameworkRepository<TEntity, TPrimeryKey>, 
		IBaseServiceRepository<TEntity, TPrimeryKey>
		where TEntity : class, IEntityIdentity<TPrimeryKey>
	{
		#region Data Members
		private EGServiceContext _ministryContext;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance from type 
		/// BaseServiceRepository.
		/// </summary>
		/// <param name="context"></param>
		public BaseServiceRepository(EGServiceContext context, ICurrentUserService currentUserService)
			: base(context, currentUserService)
		{
			this.MinistryContext = context;
		}
		#endregion

		#region Properties
		protected EGServiceContext MinistryContext
		{
			get { return this._ministryContext; }
			private set { this._ministryContext = value; }
		}
		#endregion
	}
}
