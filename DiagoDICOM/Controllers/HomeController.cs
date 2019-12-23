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

namespace DiagoDICOM.Controllers
{
	
	[SessionTimeOut]
	public class HomeController : Controller
	{
		DataAccess da = new DataAccess();
		private static IHttpContextAccessor _httpContextAccessor;
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

		public IActionResult Logs()
		{
			var data = da.GetLogs();
			return View("Logs", data);
		}

		[HttpPost]
		public IActionResult DeleteStudies(string studyIds)
		{
			da.DeleteStudies(studyIds);
			return Json("Success");
		}
	}
}
