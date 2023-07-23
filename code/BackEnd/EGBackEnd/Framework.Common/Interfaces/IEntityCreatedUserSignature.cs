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
	public interface IEntityCreatedUserSignature
	{
		#region Properties
		/// <summary>
		/// Gets or sets entity CreatedByUserId
		/// </summary>
		long? CreatedByUserId { get; set; }
		#endregion
	}
}
