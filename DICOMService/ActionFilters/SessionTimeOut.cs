using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DICOMService.Common;

namespace DICOMService.ActionFilters
{
	public class SessionTimeOut : ActionFilterAttribute
	{

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var context = filterContext.HttpContext;
			if (string.IsNullOrEmpty(context.Session.GetString("UserId")))
			{
				filterContext.Result = new RedirectResult("~/Account/LogIn");
			}
			AppUser.UserName = context.Session.GetString("UserName");
			base.OnActionExecuting(filterContext);
		}
	}
}
