using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcBoostrap.Models;

namespace MvcBoostrap.DAL
{
	public class MVCContext : DbContext
	{
		public MVCContext(DbContextOptions<MVCContext> options)
			: base(options)
		{
		}

		public DbSet<UserModel> Users { get; set; }
		public DbSet<UserManagerModel> UserManagers { get; set; }
		public DbSet<SampleItemModel> SampleItems { get; set; }

		public const string DEFAULT_USER_EMAIL = "ramon@misoftware.com.br";
		public const string DEFAULT_USER_PWD = "SEnha123";

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserManagerModel>(b =>
			{
				b.HasIndex(b => b.Email).IsUnique();

				b.HasData(new UserManagerModel()
				{
					Id = 1,
					Name = "Ramon",
					Email = DEFAULT_USER_EMAIL,
					PWD = DEFAULT_USER_PWD,
				});
			});

			modelBuilder.Entity<SampleItemModel>(b =>
			{
				for(int i = 1; i < 21; i++)
				{
					b.HasData(new SampleItemModel()
					{
						Id = i,
						Name = "Hello!!",
						Price = 10*i
					});
				}
			});
		}
	}
}
