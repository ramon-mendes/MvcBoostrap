using Microsoft.AspNetCore.Mvc;
using MvcBoostrap.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcBoostrap.Controllers
{
	public class ContatoController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(string name, string email, string msg)
		{
			Mailing.SendTheMasterMail("Contact", $@"Nomr: {name}
E-mail: {email}
Msg: {msg}");
			Success("Mensagem enviada com sucesso!");
			return RedirectToAction(nameof(Index));
		}
	}
}