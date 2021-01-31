using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcBoostrap.Classes;
using MvcBoostrap.Controllers;
using MvcBoostrap.DAL;
using MvcBoostrap.Models;

namespace MvcBoostrap.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AuthController : BaseController
	{
		public AuthController(MVCContext db)
		{
			_db = db;
		}

		public IActionResult Login(string ReturnUrl)
		{
			return View(new UserManagerModel() { ReturnUrl = ReturnUrl });
		}

		[HttpPost]
		public IActionResult Login(UserManagerModel model)
		{
			if(AuthManager.Login(HttpContext, model.Email, model.PWD, _db))
			{
				if(model.ReturnUrl != null)
					return Redirect(model.ReturnUrl);
				return Redirect("/Admin");
			}

			var res = new UserManagerValidator().Validate(model);
			if(!res.IsValid)
			{
				res.AddToModelState(ModelState, null);
				return View(model);
			}

			return View(model);
		}

		public IActionResult Logout()
		{
			AuthManager.Logout(HttpContext);
			return RedirectToAction(nameof(Login));
		}
	}
}