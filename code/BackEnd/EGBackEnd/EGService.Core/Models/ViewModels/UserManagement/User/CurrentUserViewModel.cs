#region Using ...
using Framework.Common.Enums;
using EGService.Common.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
#endregion

/*


*/
namespace EGService.Core.Models.ViewModels
{
    /// <summary>
    /// 
    /// </summary>

    public class CurrentUserViewModel : Base.BaseViewModel
    {
       
        /// <summary>
        /// Initializes a new instance from type
        /// UserViewModel
        /// </summary>
        public CurrentUserViewModel()
        {
            
        }
        
        public long Id { get; set; }
      
        public System.String Name { get; set; }
        //public string Location { get; set; }
    }
}
