using EGService.Core.Common;
using EGService.Core.Helper;
using EGService.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGService.DataAccess.Seed
{
	public class UserSeed
	{

		static UserSeed()
		{
			Seed();
		}

		private static void Seed()
		{
			SeedList = new List<User>();

			if (ApplicationGlobalConfig.EnableSeedOnMigration)
			{

				DateTime now = new DateTime(2022, 9, 8, 13, 15, 24, 581, DateTimeKind.Local);


				List<User> entityList = new List<User>() {
				new User
				{
					Id = 1,
					CreationDate = now,
					LastName = "admin",
                    FirstName = "admin",
					Username = "admin",
					IsActive = true,
					IsDeleted = false,
					Password = HashPass.HashPassword("P@ssw0rd")
				}

				};

				SeedList = entityList;
			}
		}

		public static List<User> SeedList { get; set; }
	}
}
