#region Using ...
using Framework.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using EGService.DataAccess.Mappings;
using EGService.Entity.Entities;
using System.Linq;
using Microsoft.Data.SqlClient;

#endregion

/*


 */
namespace EGService.DataAccess.Contexts
{
    /// <summary>
    /// 
    /// </summary>
    public class EGServiceContext : DbContext
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance from type 
        /// EGServiceContext.
        /// </summary>
        /// <param name="options"></param>
        public EGServiceContext(DbContextOptions options)
            : base(options)
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Override this method to further configure the model that was discovered by convention
        /// from the entity types exposed in Microsoft.EntityFrameworkCore.DbSet`1 properties
        /// on your derived context. The resulting model may be cached and re-used for subsequent
        /// instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder being used to construct the model for this context.Databases(and
        /// other extensions) typically define extension methods on this object that allow
        /// you to configure aspects of the model that are specific to a given database.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
    
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(sensitiveDataLoggingEnabled: true);
        }
        #endregion

        #region Properties
        public DbSet<User> Users { get; set; }
       

       

        #endregion
    }
}
