#region Using ...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EGService.Core.Common;
using EGService.Core.Helper;
using EGService.DataAccess.Seed;
using EGService.Entity.Entities;
#endregion

/*


*/
namespace EGService.DataAccess.Mappings
{
	/// <summary>
	/// Provides a configuration for entity 
	/// User
	/// that will called when model created.
	/// </summary>
	public class UserMap : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<User>
	{
		public static List<User> Users;

		#region Methods

		/// <summary>
		/// Configures the entity of type TEntity.
		/// </summary>
		/// <param name="builder">
		/// The builder to be used to configure the entity type.
		/// </param>
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable($"{typeof(User).Name}s", ApplicationGlobalConfig.Schema.UserManagementSchema);

			#region Configure Fields
			builder.Property(prop => prop.FirstName).HasMaxLength(200);
			builder.Property(prop => prop.LastName).HasMaxLength(200);
			builder.Property(prop => prop.Email).HasMaxLength(200);
			builder.Property(prop => prop.Password).HasMaxLength(200);
			builder.Property(prop => prop.Username).HasMaxLength(200);


			builder.HasIndex(prop => prop.Username);//.IsUnique();
			builder.HasIndex(prop => prop.LastName);
			#endregion

			#region Configure Relations
			

			#endregion

			#region Ignore
			/* 
             * If do you want to initialize some data in 
             * this entity use this method: 
             * builder.Ignore(entity => entity.<PropertyName>);
             */
			#endregion

			#region Set Initial Data
			/* 
             * If do you want to initialize some data in
             * this entity use this method: 
             * builder.HasData(); 
             */
			this.Seed(builder);
			#endregion
		}

		/// <summary>
		/// Initialize databas entity with some data.
		/// </summary>
		/// <param name="builder"></param>
		private void Seed(EntityTypeBuilder<User> builder)
		{
			if (ApplicationGlobalConfig.EnableSeedOnMigration)
			{
				//DateTime now = DateTime.Now;
				//int idCounter = 0;

				//entityList.AddRange(new User[]
				//{
				//new User
				//{
				//Id = 1,
				//CreationDate = now,
				//Username = "",
				//Password = "",
				//IsActive = false,
				//},
				//new User
				//{
				//Id = 2,
				//CreationDate = now,
				//ParentTranslatedUserId = 1,
				//Language = Language.English,
				//FirstName = "",
				//MiddleName = "",
				//LastName = "",
				//
				//},
				//new User
				//{
				//Id = 3,
				//CreationDate = now,
				//ParentTranslatedUserId = 1,
				//Language = Language.Arabic,
				//FirstName = "",
				//MiddleName = "",
				//LastName = "",
				//
				//},
				//});


				builder.HasData(UserSeed.SeedList);
			}
		}
		#endregion
	}
}
