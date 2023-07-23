#region Using ...
using Framework.Common.Enums;
using EGService.Common.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
#endregion

/*


*/
namespace EGService.Core.Models.ViewModels
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
