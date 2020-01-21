using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DICOMService.Common;
using Newtonsoft.Json;
using DICOMService.Models;
using DICOMService.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Web;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace DICOMService.Controllers
{
	
	public class AccountController : Controller
	{
		DataAccess da = new DataAccess();
		IConfiguration _config;
		private IHttpContextAccessor _httpContextAccessor;
		public AccountController(IHttpContextAccessor httpContextAccessor, IConfiguration config)
		{
			_httpContextAccessor = httpContextAccessor;
			_config = config;
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
					    if (context.Session.GetString("ChangePassword") == "true")
					    {
					    	ViewBag.IsChangePassword = true;
					    }
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
					if (context.Session.GetString("ForgotPassword") == "true")
					{
						ViewBag.IsForgotPassword = true;
					}
						message = "Invalid User";
						ViewBag.Message = message;
						return View();
					}
					else
					{
					if (MailSend(userData))
					{
						message = "A request for the password change has been sent to your mail id.Please click <a href=" + AppUser.AppUrl + "/Account/LogIn" + ">here</a> to LogIn";
						ViewBag.Message = message;
						return View("Message");
					}
					else
					{
                        ViewBag.Message = "InValid SMTP Credentials";
						return View();
					}
					}
				}


			    context.Session.SetString("ForgotPassword", "");			    
			    context.Session.SetString("ChangePassword", "");
			    context.Session.SetString("UserId", userData.UserId.ToString());
				context.Session.SetString("UserName", userData.UserName.ToString());
				return RedirectToAction("Home", "Home");
			
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
			var context = _httpContextAccessor.HttpContext;
			context.Session.SetString("ChangePassword", "true");
			ViewBag.IsChangePassword = true;
			return View("LogOn");
		}
		public IActionResult ForgotPassword()
		{
			var context = _httpContextAccessor.HttpContext;
			ViewBag.IsForgotPassword = true;
			AppUser.UserName = string.Empty;
			context.Session.SetString("ForgotPassword","true");
			return View("LogOn");
		}

		public bool MailSend(Users user)
		{
			try
			{
				string body = string.Empty;
				string FromMail = _config["ApplicationSettings:FromEmail"];
				string Password = _config["ApplicationSettings:Password"];
				MailMessage msg = new MailMessage();
				msg.From = new MailAddress(FromMail);
				msg.To.Add(user.EmailId);
				msg.Subject = "Request For Forgot Password";
				msg.IsBodyHtml = true;

				if (user != null)
				{
					body = "Hi " + user.UserName + ", <br/>" +
							"Your account password has been changed as per your request.The password is <b>" + user.Password + "</b>.";
					msg.Body = body;
				}
				else
				{
					body = "";
					msg.Body = body;

				}

				var client = new SmtpClient()
				{
					Port = 587,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					Host = _config["ApplicationSettings:SMTPServer"],
					EnableSsl = Convert.ToBoolean(_config["ApplicationSettings:SMTPUseSsl"]),
					Credentials = new NetworkCredential( FromMail , Password )
				};
				client.Send(msg);
				return true;
			}
			catch (Exception e)
			{
				Logs.WriteToLogFile(e.ToString());
				return false;
			}
		}
	}
}