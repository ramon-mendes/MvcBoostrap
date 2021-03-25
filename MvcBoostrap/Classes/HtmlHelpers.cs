using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcBoostrap.Classes
{
	public static class HtmlHelpers
	{
		//public static string Translate(this HtmlHelper helper, string txt_pt) => I18N.GlobalTranslate(txt_pt); -- use I18N.Translate

		public static object PrintIf(this IHtmlHelper helper, bool cond, string html, string elsehtml = null)
		{
			if(cond)
				return helper.Raw(html);
			if(elsehtml != null)
				return helper.Raw(elsehtml);
			return null;
		}

		public static object PrintIf(this IHtmlHelper helper, bool cond, Func<string> html, Func<string> elsehtml = null)
		{
			if(cond)
				return helper.Raw(html());
			if(elsehtml != null)
				return helper.Raw(elsehtml());
			return null;
		}

		public static IHtmlContent Breadcrumb(this IHtmlHelper helper,
			string current,
			string prev_name1 = null, string prev_url1 = null,
			string prev_name2 = null, string prev_url2 = null,
			string prev_name3 = null, string prev_url3 = null
		)
		{
			var list = new List<(string Name, string Url)>();
			if(prev_name3 != null)
				list.Add((prev_name3, prev_url3));
			if(prev_name2 != null)
				list.Add((prev_name2, prev_url2));
			if(prev_name1 != null)
				list.Add((prev_name1, prev_url1));
			list.Add((current, null));

			return helper.Partial("_Breadcrumb", list);
		}

		public static IHtmlContent Translate(this IHtmlHelper helper, string key_en)
		{
			return new HtmlString(I18N.GlobalTranslate(key_en));
		}

		public static string ToHTMLDate(this DateTime dt)
		{
			return dt.ToString("yyyy-MM-dd");
		}
	}
}