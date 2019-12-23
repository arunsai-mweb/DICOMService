using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagoDICOM.Common
{
	public static class AppUser
	{
		public static int? UserId
		{
			get;
			set;
		}
		public static string UserName
		{
			get;
			set;
		}
		public static string EmailId
		{
			get;
			set;
		}
		public static string Password
		{
			get;
			set;
		}
		public static string ConnectionString
		{
			get;set;
		}
		public static string AppUrl
		{
			get;set;
		}
		public static string WebRootPath { get; set; }
		
	}
}
