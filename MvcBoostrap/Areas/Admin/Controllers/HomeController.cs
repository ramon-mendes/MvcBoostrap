using Microsoft.AspNetCore.Mvc;
using MvcBoostrap.Controllers;
using MvcBoostrap.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcBoostrap.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : BaseController
	{
		public HomeController(MVCContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
