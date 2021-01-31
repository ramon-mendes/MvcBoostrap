using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FILE = System.IO.File;

namespace MvcBoostrap.Controllers
{
	public class BaseCRUDController<TEntity> : BaseController where TEntity : class, new()
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);
		}

		public IActionResult RetAddView(TEntity model = null)
		{
			ViewBag.is_create = true;
			return View("CreateEdit", model ?? new TEntity());
		}

		public IActionResult RetEditView(TEntity model)
		{
			ViewBag.is_create = false;
			return View("CreateEdit", model);
		}

		protected TEntity UpdateModel(int id, Action<TEntity> change_cbk)
		{
			var set = _db.Set<TEntity>();
			var model = set.Find(id);
			_db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

			change_cbk(model);
			_db.SaveChanges();

			Success(I18N.GlobalTranslate("Saved with success!"));
			return model;
		}

		protected TEntity DeleteModel(int id)
		{
			var set = _db.Set<TEntity>();
			var model = set.Find(id);
			set.Remove(model);
			_db.SaveChanges();

			Success(I18N.GlobalTranslate("Deleted with success."));
			return model;
		}
	}
}