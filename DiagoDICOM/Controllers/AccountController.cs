using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DiagoDICOM.Common;
using Newtonsoft.Json;
using DiagoDICOM.Models;
using DiagoDICOM.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace DiagoDICOM.Controllers
{
	
	public class AccountController : Controller
	{
		DataAccess da = new DataAccess();
		private IHttpContextAccessor _httpContextAccessor;
		public AccountController(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

	    public IActionResult LogIn()
        {
			AppUser.AppUrl = this.Request.Scheme.ToString() + "://" + this.Request.Host.ToString();
			ViewBag.Users = da.GetUsers();
			return View("LogOn");
        }

		public IActionResult LogOn(Users user)
		{
			
				var context = _httpContextAccessor.HttpContext;
				Users userData = null;
				var message = string.Empty;
				if (!user.IsChangePassword && !user.IsForgotPassword)
				{
					userData = da.CheckIsExistingUser(user);
					if (userData == null)
					{
						message = "Invalid User or Password";
						ViewBag.Message = message;
						return View();
					}
					if (user.IsInitial)
					{
						message = "User has been created successfully.Please click <a href=" + AppUser.AppUrl + "/Account/LogIn" + ">here</a> to LogIn";
						ViewBag.UserName = userData.UserName;
						ViewBag.Message = message;
						return View("Message");
					}

				}
				else if (user.IsChangePassword)
				{
					userData = da.CheckIsExistingUser(user);
					if (userData == null)
					{
						message = "Invalid User or Password";
						ViewBag.Message = message;
						return View();
					}
					else
					{
						message = "Password has been changed successfully.Please click <a href=" + AppUser.AppUrl + "/Account/LogIn" + ">here</a> to LogIn";
						ViewBag.Message = message;
						return View("Message");
					}
				}
				else if (user.IsForgotPassword)
				{
					userData = da.CheckIsExistingUser(user);
					if (userData == null)
					{
						message = "Invalid User or Password";
						ViewBag.Message = message;
						return View();
					}
					else
					{
						message = "Password has been resetted successfully.Please click <a href=" + AppUser.AppUrl + "/Account/LogIn" + ">here</a> to LogIn";
						ViewBag.Message = message;
						return View("Message");
					}
				}


				context.Session.SetString("UserId", userData.UserId.ToString());
				context.Session.SetString("UserName", userData.UserName.ToString());
				return RedirectToAction("Index", "Home");
			
		}

		public IActionResult LogOut()
		{
			var context = _httpContextAccessor.HttpContext;

			AppUser.UserName = string.Empty;
			AppUser.UserId = null;
			context.Session.SetString("UserId", "");
			context.Session.SetString("UserName", "");
			return View("LogOn");
		}

		public IActionResult ChangePassword()
		{
			ViewBag.IsChangePassword = true;
			return View("LogOn");
		}
		public IActionResult ForgotPassword()
		{
			ViewBag.IsForgotPassword = true;
			AppUser.UserName = string.Empty;
			
			return View("LogOn");
		}
	}
}