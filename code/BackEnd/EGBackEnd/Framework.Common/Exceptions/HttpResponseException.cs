#region Using ...
using System;
using System.Collections.Generic;
using System.Text;
#endregion

/*


 */
namespace Framework.Common.Exceptions
{
    public class HttpResponseException : Exception
    {
        public int Status { get; set; } = 500;

        public object Value { get; set; }
    }
}
