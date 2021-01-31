using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MvcBoostrap.DAL;
using MvcBoostrap.Models;
using DB = MvcBoostrap.DAL.MVCContext;

namespace MvcBoostrap.Classes
{
	public static class AuthManager
	{
		static readonly string COOKIE_NAME = "LID";
		static readonly TimeSpan LOGIN_TIMEOUT = TimeSpan.FromDays(2);
		public static UserManagerModel UserLogged { get; private set; }

		private static Dictionary<string, Tuple<int, DateTime>> _id2user = new Dictionary<string, Tuple<int, DateTime>>();

		public static bool IsLogged(this HttpContext ctx)
		{
			var guid = ctx.Request.Cookies[COOKIE_NAME];
			if(guid == null)
				return false;

			if(!_id2user.ContainsKey(guid))
				return false;
			var login = _id2user[guid];
			if(login.Item2 < DateTime.Now)
				return false;
			return true;
		}

		public static UserManagerModel LoadLoggedUser(this HttpContext ctx, DB db)
		{
			if(!IsLogged(ctx))
				return null;

			var guid = ctx.Request.Cookies[COOKIE_NAME];
			var user_id = _id2user[guid].Item1;
			UserLogged = db.UserManagers.Find(user_id);
			return UserLogged;
		}

		public static bool Login(this HttpContext ctx, string email, string pwd, DB db)
		{
			if(email == null || pwd == null)
				return false;
			email = email.Trim();

			//string pwd_hash = ComputeHash(pwd);
			var user = db.UserManagers.FirstOrDefault(u => u.Email == email && u.PWD == pwd);
			if(user == null)
				return false;

			var lid = new Guid().ToString();
			var dt_expires = DateTime.Now.Add(LOGIN_TIMEOUT);
			_id2user[lid] = new Tuple<int, DateTime>(user.Id, dt_expires);
			ctx.Response.Cookies.Append(COOKIE_NAME, lid, new CookieOptions() { Expires = new DateTimeOffset(dt_expires) });
			return true;
		}

		/*public static bool LoginBySlug(this HttpContext ctx, string slug, DB db)
		{
			var user = db.Users.FirstOrDefault(u => u.slug_id == slug);
			if(user == null)
				return false;

			var lid = new Guid().ToString();
			var dt_expires = DateTime.Now.Add(LOGIN_TIMEOUT);
			_id2user[lid] = new Tuple<int, DateTime>(user.Id, dt_expires);
			ctx.Response.Cookies.Append(COOKIE_NAME, lid, new CookieOptions() { Expires = new DateTimeOffset(dt_expires) });
			return true;
		}*/

		public static void Logout(this HttpContext ctx)
		{
			var guid = ctx.Request.Cookies[COOKIE_NAME];

			ctx.Response.Cookies.Delete(COOKIE_NAME);
			if(guid != null)
				_id2user.Remove(guid);
		}

		public static string ComputeHash(string input)
		{
			HashAlgorithm algorithm = new MD5CryptoServiceProvider();
			string salt = "MeowMidiPopup3D";
			byte[] inputBytes = Encoding.UTF8.GetBytes(input);
			byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

			// Combine salt and input bytes
			byte[] saltedInput = new byte[saltBytes.Length + inputBytes.Length];
			saltBytes.CopyTo(saltedInput, 0);
			inputBytes.CopyTo(saltedInput, salt.Length);

			byte[] hashedBytes = algorithm.ComputeHash(saltedInput);
			return BitConverter.ToString(hashedBytes);
		}
	}
}