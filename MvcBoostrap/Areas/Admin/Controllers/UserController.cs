using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using MvcBoostrap.Classes;
using MvcBoostrap.Controllers;
using MvcBoostrap.DAL;
using MvcBoostrap.Models;
using X.PagedList;

namespace MvcBoostrap.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class UserController : BaseCRUDController<UserModel>
	{
		public UserController(MVCContext db)
		{
			_db = db;
		}

		// GET: /Admin/User/List
		public IActionResult List(int? page)
		{
			var list = _db.Users
				.OrderByDescending(c => c.Name)
				.ToPagedList(page ?? 1, Consts.PAGINATION_ITENS_PER_PAGE);
			return View(list);
		}

		// GET: /Admin/User/Add
		public IActionResult Add()
		{
			return RetAddView(new UserModel());
		}

		// POST: /Admin/User/Add
		[HttpPost]
		public IActionResult Add(UserModel model)
		{
			var res = new UserValidator().Validate(model);
			if(!res.IsValid)
			{
				res.AddToModelState(ModelState, null);
				return RetAddView(model);
			}

			_db.Users.Add(model);
			_db.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		// GET: /Admin/User/Edit/{ID}
		public IActionResult Edit(int id)
		{
			var model = _db.Users.Find(id);
			model.PWD2 = model.PWD;
			return RetEditView(model);
		}

		// POST: /Admin/User/Edit/{ID}
		[HttpPost]
		public IActionResult Edit(UserModel model)
		{
			var res = new UserValidator().Validate(model);
			if(!res.IsValid)
			{
				res.AddToModelState(ModelState, null);
				return RetEditView(model);
			}

			UpdateModel(model.Id, entry =>
			{
				entry.Name = model.Name;
				entry.Email = model.Email;
				entry.PWD = model.PWD;
			});

			return RedirectToAction(nameof(List));
		}

		// GET: /Admin/User/Delete/{ID}
		public IActionResult Delete(int id)
		{
			DeleteModel(id);
			return RedirectToAction(nameof(List));
		}
	}
}
