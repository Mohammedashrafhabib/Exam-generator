#region Using ...
using System;
using System.Collections.Generic;
using System.Text;
#endregion

/*


 */
namespace Framework.Common.Interfaces
{
	/// <summary>
	/// 
	/// </summary>
	public interface IDateTimeSignature : ICreationTimeSignature
	{
		#region Properties
		DateTime? FirstModificationDate { get; set; }
		DateTime? LastModificationDate { get; set; }
		#endregion
	}
}
