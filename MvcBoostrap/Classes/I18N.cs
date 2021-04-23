using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Html;
using LiteDB;
using MvcBoostrap.DAL;
using MvcBoostrap.Models;
using MvcBoostrap.Classes;
using MvcBoostrap;

namespace MvcBoostrap.Classes
{
	public class I18N
	{
		private static I18N _singleton;
		private static LiteDbContext _db = new LiteDbContext();
		private IHttpContextAccessor _httpContextAccessor;

		public I18N(IHttpContextAccessor httpContextAccessor)
		{
			_singleton = this;
			_httpContextAccessor = httpContextAccessor;
		}

		public static void DoTransaction(Action<ILiteCollection<TranslationModel>> cbk)
		{
			lock(_db)
			{
				cbk(_db.Translations);
			}
		}

		public static string GlobalTranslate(string key_pt)
		{
			return _singleton.Translate(key_pt);
		}

		public IHtmlContent B2S(bool b)
		{
			return b ? new HtmlString(Translate("Sim")) : new HtmlString("<span class='text-danger'>" + Translate("Não") + "</span>");
		}

		public string Translate(string key_pt)
		{
			lock(_db)
			{
				var reg = _db.Translations.FindOne(i => i.key_pt == key_pt);
				if(reg == null)
				{
					_db.Translations.Insert(new TranslationModel
					{
						key_pt = key_pt
					});

					return key_pt;
				}

				string translated = null;
				switch(GetCurrentIdiom())
				{
					case EIdiom.INVALID:
					case EIdiom.PT: return key_pt;
					case EIdiom.EN: translated = reg.trans_en; break;
					case EIdiom.ES: translated = reg.trans_es; break;
					case EIdiom.FR: translated = reg.trans_fr; break;
					case EIdiom.DE: translated = reg.trans_al; break;
					case EIdiom.IT: translated = reg.trans_it; break;
					default:
						Debug.Assert(false);
						break;
				}
				return translated != null ? translated : key_pt;
			}
		}

		public static Dictionary<string, string> TranslationMap()
		{
			lock(_db)
			{
				Dictionary<string, string> res = new Dictionary<string, string>();
				foreach(var item in _db.Translations.FindAll())
				{
					string translation = null;
					switch(_singleton.GetCurrentIdiom())
					{
						case EIdiom.PT: translation = item.key_pt; break;
						case EIdiom.EN: translation = item.trans_en; break;
						case EIdiom.ES: translation = item.trans_es; break;
						case EIdiom.IT: translation = item.trans_it; break;
						case EIdiom.DE: translation = item.trans_al; break;
						case EIdiom.FR: translation = item.trans_fr; break;
					}
					res[item.key_pt] = translation == null ? item.key_pt : translation;
				}
				return res;
			}
		}


		public static List<TranslationModel> AllTranslations()
		{
			lock(_db)
			{
				return _db.Translations.FindAll().ToList();
			}
		}

		public EIdiom GetCurrentIdiom()
		{
			using(var scope = Startup._provider.CreateScope())
			{
				return EIdiom.PT;
				/*
				HttpContext ctx = _httpContextAccessor.HttpContext;
				if(!AuthManager.IsLogged(ctx))
				{
					var userLangs = ctx.Request.Headers["Accept-Language"].ToString();
					var firstLang = userLangs.ToLower().Split(',').FirstOrDefault();
					var defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;

					if(firstLang.StartsWith("en"))
						return EIdiom.EN;
					if(firstLang.StartsWith("es"))
						return EIdiom.ES;
					if(firstLang.StartsWith("pt"))
						return EIdiom.PT;
					if(firstLang.StartsWith("fr"))
						return EIdiom.FR;
					if(firstLang.StartsWith("it"))
						return EIdiom.IT;
					if(firstLang.StartsWith("de"))
						return EIdiom.DE;
				}

				var db = scope.ServiceProvider.GetRequiredService<MCContext>();
				return AuthManager.LoadLoggedUser(ctx, db).Idiom;*/
			}
		}
	}
}