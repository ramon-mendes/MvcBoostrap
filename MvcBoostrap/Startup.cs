using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcBoostrap.Classes;
using MvcBoostrap.DAL;

namespace MvcBoostrap
{
	public class Startup
	{
		public static IWebHostEnvironment _env { get; private set; }
		public static MVCContext _db { get; private set; }
		public static IServiceProvider _provider { get; private set; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// https://stackoverflow.com/questions/54600273/net-core-3-0-preview-2-razor-views-dont-automatically-recompile-on-change
			services.AddControllersWithViews(options =>
			{
				options.Filters.Add(typeof(AuthFilter));
			}).AddRazorRuntimeCompilation();

			services.AddHttpContextAccessor();

			services.AddDbContext<MVCContext>(options =>
				options.UseSqlite(Configuration.GetConnectionString("conn"))
			);

			services.AddSession();

			services.AddTransient<I18N>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MVCContext db, IServiceProvider serviceProvider)
		{
			_env = env;
			_db = db;
			_provider = app.ApplicationServices;

			//db.Database.Migrate();
			db.Database.EnsureCreated();

			var t1 = db.Users.ToList();
			var t2 = db.UserManagers.ToList();

			serviceProvider.GetRequiredService<I18N>();

			//if(env.IsDevelopment())
			if(true)
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapAreaControllerRoute(
					name: "areas",
					areaName: "Admin",
					pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
				);

				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}

		public static string MapPath(string path)
		{
			if(path[0] == '~')
				return _env.WebRootPath + '/' + path.Substring(1);
			return _env.ContentRootPath + '/' + path;
		}
	}
}
