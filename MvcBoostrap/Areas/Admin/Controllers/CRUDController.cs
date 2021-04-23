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
	public class CRUDController : BaseCRUDController<SampleItemModel>
	{
		public CRUDController(MVCContext db)
		{
			_db = db;
		}

		private void SetupSearch(UI_ItensSearch search = null)
		{
			ViewBag.search = search ?? new UI_ItensSearch();
			ViewBag.in_search = search != null;
		}

		// GET: /Admin/CRUD/List
		public IActionResult List(int? page)
		{
			SetupSearch();

			var list = _db.SampleItems
				.OrderByDescending(c => c.Dt)
				.ToPagedList(page ?? 1, Consts.PAGINATION_ITENS_PER_PAGE);
			return View(list);
		}

		// POST: /Admin/CRUD/List
		[HttpPost]
		public IActionResult List(UI_ItensSearch search)
		{
			var query = _db.SampleItems.AsQueryable();
			if(search.Name != null)
			{
				search.Name = search.Name.ToLower();
				query = query.Where(p => p.Name.ToLower().Contains(search.Name));
			}

			SetupSearch(search);

			var list = query.OrderByDescending(p => p.Dt).ToList();
			return View(list);
		}

		// GET: /Admin/CRUD/Add
		public IActionResult Add()
		{
			return RetAddView(new SampleItemModel() { Dt = DateTime.Now.Date });
		}

		// POST: /Admin/CRUD/Add
		[HttpPost]
		public IActionResult Add(SampleItemModel model)
		{
			var res = new SampleItemValidator().Validate(model);
			if(!res.IsValid)
			{
				res.AddToModelState(ModelState, null);
				return RetAddView(model);
			}

			_db.SampleItems.Add(model);
			_db.SaveChanges();

			return RedirectToAction(nameof(List));
		}

		// GET: /Admin/CRUD/Edit/{ID}
		public IActionResult Edit(int id)
		{
			var model = _db.SampleItems.Find(id);
			return RetEditView(model);
		}

		// POST: /Admin/CRUD/Edit/{ID}
		[HttpPost]
		public IActionResult Edit(SampleItemModel model)
		{
			var res = new SampleItemValidator().Validate(model);
			if(!res.IsValid)
			{
				res.AddToModelState(ModelState, null);
				return RetEditView(model);
			}

			UpdateModel(model.Id, entry =>
			{
				entry.Name = model.Name;
				entry.Dt = model.Dt;
				entry.Price = model.Price;
			});

			return RedirectToAction(nameof(List));
		}

		// GET: /Admin/CRUD/Delete/{ID}
		public IActionResult Delete(int id)
		{
			DeleteModel(id);
			return RedirectToAction(nameof(List));
		}
	}
}