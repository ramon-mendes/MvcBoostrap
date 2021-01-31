using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MvcBoostrap.DAL;

namespace MvcBoostrap.Classes
{
	class AuthFilter : IAuthorizationFilter
	{
		private MVCContext _db;

		public AuthFilter(MVCContext db)
		{
			_db = db;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if((string)context.RouteData.Values["area"] == "Admin")
			{
				if((string)context.RouteData.Values["controller"] == "Auth")
					return;

				if(!AuthManager.IsLogged(context.HttpContext))
				{
#if false && DEBUG
					AuthManager.Login(context.HttpContext, MCContext.DEFAULT_USER_EMAIL, MCContext.DEFAULT_USER_PWD, _db);
					return;
#endif

					context.Result = new RedirectToActionResult("Login", "Auth", new { Area = "Admin", ReturnUrl = context.HttpContext.Request.Path });
				}
			}
		}
	}
}