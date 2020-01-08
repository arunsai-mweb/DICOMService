using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DiagoDICOM.Models;
using DiagoDICOM.Common;
using DiagoDICOM.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using DiagoDICOM.ActionFilters;
using Microsoft.AspNetCore.Http;
using System.IO.Compression;
using System.IO;
namespace DiagoDICOM.Controllers
{
	
	[SessionTimeOut]
	public class HomeController : Controller
	{
		DataAccess da = new DataAccess();
		private static IHttpContextAccessor _httpContextAccessor;

		public object DICOMModals { get; private set; }

		public HomeController(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		
		public IActionResult Index(int? destinationId,int? clientId)
		{
			ViewData["Destinations"] = da.GetDestinations().Select(i => new SelectListItem
			{
				Text = i.DestinationName,
				Value = i.DestinationName
			}).ToList();
			ViewData["Clients"] = da.GetClients().Select(i => new SelectListItem
			{
				Text = i.ClientName,
				Value = i.ClientName
			}).ToList();
			List<CaseStudies> studies = da.GetClientCases(clientId, destinationId);
			return View("Index", studies);
		}

		public IActionResult AddEditDestination(int? id)
		{

			ViewBag.Clients = da.GetClients();
			ViewBag.Destinations = da.GetDestinations().Select(x=> new { x.DestinationId , x.PushToIpAddress,x.PushToAETitle,x.PushToPort });
			var model = new Destination();
			if (id != null)
			{
				model = da.GetDestinationById(id);
			}

			return PartialView("AddEditDestination",model);
		}

		public IActionResult SaveDestination(Destination dest)
		{
			da.SaveDestination(dest,AppUser.UserId);
			return RedirectToAction("Destinations");
		}

		[HttpPost]
		public IActionResult DeleteDestination(int id)
		{
			da.DeleteDestination(id);
			return Json("Success");
		}

		public IActionResult Destinations(int? clientId = null,int? destinationId = null)
		{
			    ViewBag.Clients = da.GetClients();
				List<Destination> destData = da.GetDestinations();
				return View("Destinations", destData);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		
		public IActionResult GetUserDetails()
		{
			return Json(AppUser.EmailId);
		}

		public IActionResult Clients()
		{
			List<Clients> lstClient = da.GetClients();
			return View("Clients", lstClient);
		}
		
		public IActionResult AddEditClient(int? clientId)
		{
			Clients cld = new Clients();
			ViewBag.Clients = da.GetClients();
			ViewData["Destinations"] = da.GetDestinations().Select(i => new SelectListItem
			{
				Text = i.DestinationName,
				Value = i.DestinationId.ToString()
			}).ToList();
			if (clientId != null)
			{
				cld = da.GetClientById(clientId);
			}
			return PartialView("AddEditClient",cld);
		}

		public IActionResult SaveClient(Clients data)
		{
			da.AddEditClient(data);
			return RedirectToAction("Clients");
		}

		public IActionResult AddEditClientRule(int clientId,string clientName,int? clientRuleId)
		{
			Clients client = new Clients();
			client.ClientId = clientId;
			client.ClientName = clientName;
			ViewData["Destinations"] = da.GetDestinations().Select(i => new SelectListItem
			{
				Text = i.DestinationName,
				Value = i.DestinationId.ToString()
			}).ToList();
			ViewBag.ClientRules = da.GetClientRules(clientId);
			if (clientRuleId != null)
			{
				client = da.GetClientRuleById(clientRuleId);
			}
			return PartialView("AddEditClientRules", client);
		}

		public JsonResult GetClientRules(int clientId)
		{
			return Json(da.GetClientRules(clientId));
		}

		public IActionResult SaveClientRule(Clients data)
		{
			da.AddEditClientRule(data);
			return RedirectToAction("Clients");
		}

		public JsonResult DeleteClient(int clientId)
		{
			da.DeleteClient(clientId);
			return Json("Success");
		}
		public JsonResult DeleteClientRule(int clientRuleId)
		{
			da.DeleteClientRule(clientRuleId);
			return Json("Success");
		}

		public JsonResult GetStudyStatus(string studyId,int clientId)
		{
			return Json(da.GetStudyStatus(studyId,clientId));
		}

		public IActionResult GetLogs()
		{
			ViewData["Destinations"] = da.GetDestinations().Select(i => new SelectListItem
			{
				Text = i.DestinationName,
				Value = i.DestinationName
			}).ToList();
			ViewData["Clients"] = da.GetClients().Select(i => new SelectListItem
			{
				Text = i.ClientName,
				Value = i.ClientName
			}).ToList();
			var data = da.GetLogs();
			return View("Logs", data);
		}

		[HttpPost]
		public IActionResult DeleteStudies(string studyIds)
		{
			da.DeleteStudies(studyIds);
			return Json("Success");
		}


		[HttpPost]
		public IActionResult DeleteLogs(string logIds)
		{
			da.DeleteLogs(logIds);
			return Json("Success");
		}

		public IActionResult Download(string app)
		{
			ViewBag.App = app;
			if (app == "Client")
			{
				ViewData["Clients"] = da.GetClients().Select(i => new SelectListItem
				{
					Text = i.ClientId.ToString(),
					Value = i.ClientId.ToString()
				}).ToList();
			}
			else
			{
				ViewData["Destinations"] = da.GetDestinations().Select(i => new SelectListItem
				{
					Text = i.DestinationId.ToString(),
					Value = i.DestinationId.ToString()
				}).ToList();
			}
			return PartialView("Download");
		}

		[HttpPost]
		public IActionResult DownLoadClientApp(string ClientId)
		{
			try
			{
				string startPath = AppUser.WebRootPath + "\\ClientApp";
				var configFile = startPath + "\\Application\\ClientConfigFile.txt";
				var cTox = System.IO.File.ReadAllText(configFile);
				cTox = cTox.Replace("ClientIdValue", ClientId);
				System.IO.File.WriteAllText(configFile, cTox);
				string zipPath = AppUser.WebRootPath + "\\ZIP";
				foreach (FileInfo f in new DirectoryInfo(zipPath).GetFiles("*.zip"))
				{
					f.Delete();
				}
				var zipFilePath = AppUser.WebRootPath + "\\ZIP\\ClientApplication.zip";
				ZipFile.CreateFromDirectory(startPath, zipFilePath, CompressionLevel.Fastest, true);

				var xToc = System.IO.File.ReadAllText(configFile);
				xToc = xToc.Replace(ClientId, "ClientIdValue");
				System.IO.File.WriteAllText(configFile, xToc);

				const string contentType = "application/zip";
				HttpContext.Response.ContentType = contentType;
				var result = new FileContentResult(System.IO.File.ReadAllBytes(zipFilePath), contentType)
				{
					FileDownloadName = "ClientApp_" + ClientId + ".zip"
				};
				return result;
			}
			catch (Exception ex)
			{
				return Json(ex == null ? "" : ex.ToString());
			}
		}

		[HttpPost]
		public IActionResult DownLoadDestinationApp(string DestinationId)
		{
			try
			{
				string startPath = AppUser.WebRootPath + "\\DestinationApp";
				var configFile = startPath + "\\Application\\DestinationConfigFile.txt";
				var cTox = System.IO.File.ReadAllText(configFile);
				cTox = cTox.Replace("DestinationIdValue", DestinationId);
				System.IO.File.WriteAllText(configFile, cTox);
				string zipPath = AppUser.WebRootPath + "\\ZIP";
				foreach (FileInfo f in new DirectoryInfo(zipPath).GetFiles("*.zip"))
				{
					f.Delete();
				}
				var zipFilePath = AppUser.WebRootPath + "\\ZIP\\DestinationApp.zip";
				ZipFile.CreateFromDirectory(startPath, zipFilePath, CompressionLevel.Fastest, true);
				const string contentType = "application/zip";
				var xToc = System.IO.File.ReadAllText(configFile);
				xToc = xToc.Replace(DestinationId, "DestinationIdValue");
				System.IO.File.WriteAllText(configFile, xToc);
				HttpContext.Response.ContentType = contentType;
				var result = new FileContentResult(System.IO.File.ReadAllBytes(zipFilePath), contentType)
				{
					FileDownloadName = "DestinationApp_" + DestinationId + ".zip"
				};
				return result;
			}
			catch (Exception ex)
			{
				return Json(ex == null ? "" : ex.ToString());
			}
		}
	}
}
