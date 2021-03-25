using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MvcBoostrap.Models;

namespace MvcBoostrap.Classes
{
	public enum EIdiom
	{
		INVALID,
		EN,
		ES,
		PT,
		IT,
		DE,
		FR,
	}

	public enum EArea
	{
		SITE,
		ADMIN,
	}

	public static class Consts
	{
#if false
		public static readonly string BASE_URL = "https://localhost:44378/";// should end with slash
#else
		public static readonly string BASE_URL = "http://momentum.fot.br/";
#endif

		public static readonly string SITE_NAME = "MvcBootstrap";
		public static readonly bool IS_WINDOWS = Environment.OSVersion.Platform == PlatformID.Win32NT;
		public static readonly CultureInfo CULTURE_EN = new CultureInfo("en-us");
		public static readonly CultureInfo CULTURE_PTBR = new CultureInfo("pt-br");
		public const int PAGINATION_ITENS_PER_PAGE = 50;

#if DEBUG
		public const bool IN_DEBUG = true;
#else
		public const bool IN_DEBUG = false;
#endif

		static Consts()
		{
			CULTURE_EN.NumberFormat.NumberDecimalSeparator = ".";
		}

		public static readonly Dictionary<EIdiom, string> LANGUAGES = new Dictionary<EIdiom, string>
		{
			[EIdiom.EN] = "English",
			[EIdiom.ES] = "Español",
			[EIdiom.PT] = "Português",
			[EIdiom.IT] = "Italiano",
			[EIdiom.DE] = "Deutsch",
			[EIdiom.FR] = "Français",
		};
	}
}