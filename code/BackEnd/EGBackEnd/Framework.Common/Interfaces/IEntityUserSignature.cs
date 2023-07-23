#region Using ...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

/*


 */
namespace Framework.Common.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public interface IEntityUserSignature : IEntityCreatedUserSignature
	{
		#region Properties
		/// <summary>
		/// Gets or sets entity FirstModifiedByUserId
		/// </summary>
		long? FirstModifiedByUserId { get; set; }
		/// <summary>
		/// Gets or sets entity LastModifiedByUserId.
		/// </summary>
		long? LastModifiedByUserId { get; set; }
		#endregion
	}
}
