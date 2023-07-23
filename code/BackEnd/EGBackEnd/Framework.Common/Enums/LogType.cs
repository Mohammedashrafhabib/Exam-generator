#region Using ...
using System;
using System.Collections.Generic;
using System.Text;
#endregion

/*


 */
namespace Framework.Common.Enums
{
	/// <summary>
	/// 
	/// </summary>
	public enum LogType : byte
	{
		/// <summary>
		/// Specifing info type for log.
		/// </summary>
		Information,
		/// <summary>
		/// Specifing warn type for log.
		/// </summary>
		Warning,
		/// <summary>
		/// Specifing error type for log.
		/// </summary>
		Error,
		/// <summary>
		/// Specifing normal type for log.
		/// </summary>
		Text
	}
}
