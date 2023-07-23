#region Using ...
using System;
using System.Collections.Generic;
using System.Text;
#endregion

/*


 */

/*


 */
namespace EGService.Common.Enums
{
	/// <summary>
	/// 
	/// </summary>
	public enum ErrorCode
	{
		NotFound = 1,
		CodeAlreadyExist = 3,
		EmailAlreadyExist = 5,
		PhoneAlreadyExist = 6,
		UserNameAlreadyExist = 8,
		PasswordIncorrect = 17,
		
		InActiveUser = 55,
        WrongEquation =35,
		InvalidOperationException=70
	}
}
