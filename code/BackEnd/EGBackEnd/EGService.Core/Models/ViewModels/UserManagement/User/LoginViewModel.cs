using System;
using System.Collections.Generic;
using System.Text;

namespace EGService.Core.Models.ViewModels
{
    public class LoginViewModel : Base.BaseViewModel
    {
        #region Properties
        public string UserName { get; set; }
        public string Password { get; set; }
        #endregion
    }
}
