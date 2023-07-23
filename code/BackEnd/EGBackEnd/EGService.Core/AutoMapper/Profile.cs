#region Using ...
using System;
using System.Collections.Generic;
using System.Text;
using Framework.Common.Enums;
using Framework.Core.Common;
using EGService.Core.Models.ViewModels;
using EGService.Entity.Entities;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using EGService.Core.Common;
using EGService.Common.Enums;

#endregion


namespace EGService.Core
{
    /* 
	 * See: https://code-maze.com/automapper-net-core/ 
	 */

    /// <summary>
    /// Provides a named configuration for maps. 
    /// Naming conventions become scoped per 
    /// profileStockItem, StockItemListViewModel
    /// </summary>
    public class Profile : AutoMapper.Profile
    {
        #region Properties
        public static IApplicationBuilder ApplicationBuilder { get; set; }
        #endregion


        #region Constructors
        /// <summary>
        /// Initializes a new instance from type 
        /// Profile.
        /// </summary>
        public Profile()
        {
            #region User
            CreateMap<User, UserViewModel>()
               
               .ReverseMap();

          
            CreateMap<User, UserViewViewModel>().ReverseMap();
            CreateMap<User, UserDetailViewModel>().ReverseMap();
            #endregion

          

           
        
        }

        #endregion
    }
}
