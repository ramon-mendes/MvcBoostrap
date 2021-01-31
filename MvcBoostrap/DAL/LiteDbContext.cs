using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using MvcBoostrap;
using MvcBoostrap.Models;

namespace MvcBoostrap.DAL
{
	public class LiteDbContext : LiteDatabase
	{
		public static readonly string DBPATH = Startup.MapPath("/App_Data/db_i18n.db");

		public LiteDbContext()
			: base(DBPATH)
		{
			Translations = GetCollection<TranslationModel>(nameof(Translations));
		}

		public ILiteCollection<TranslationModel> Translations;
	}
}