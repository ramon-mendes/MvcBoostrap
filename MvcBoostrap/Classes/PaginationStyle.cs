using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using X.PagedList.Mvc;
using X.PagedList.Mvc.Core.Common;
using X.PagedList.Web.Common;

namespace MvcBoostrap.Classes
{
	public static class PaginationStyle
	{
		static PaginationStyle()
		{
			MIPaging = new PagedListRenderOptions
			{
				DisplayLinkToFirstPage = PagedListDisplayMode.Never,
				DisplayLinkToLastPage = PagedListDisplayMode.Never,
				DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
				DisplayLinkToNextPage = PagedListDisplayMode.Always,
				DisplayLinkToIndividualPages = true,
				LiElementClasses = new[] { "page-item" },
				PageClasses = new[] { "page-link" },
			};
		}

		public static readonly PagedListRenderOptions MIPaging;
	}
}