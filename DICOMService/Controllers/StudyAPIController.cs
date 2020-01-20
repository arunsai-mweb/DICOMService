using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DICOMService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Dynamic;
using System.Xml;
using System.Xml.Serialization;
using DICOMService.Models;

namespace DICOMService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyAPIController : ControllerBase
    {
		DataAccess da = new DataAccess();
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public IActionResult Post([FromBody]CaseStudies cs)
        {

			if (!String.IsNullOrEmpty(cs.ScanDate) && !String.IsNullOrEmpty(cs.ScanTime))
				cs.ScanDate = Convert.ToString(DateTime.Parse(DateTime.ParseExact((cs.ScanDate + cs.ScanTime).Substring(0, 12), "yyyyMMddHHmm", null).ToString("yyyy-MM-dd hh:mm tt")).ToString("yyyy-MM-dd hh:mm:ss"));
			var modal = da.SaveImageStudy(cs);
			if (!string.IsNullOrEmpty(modal.DestinationId))
			{
				return StatusCode((int)HttpStatusCode.OK, JsonConvert.SerializeObject(modal));
			}
			else
			{
				da.SaveLogDetails("INFO", DateTime.Now.ToString() + " DICOM Study Saved but no destination found for the modality or clientId may be wrong", cs.SOPInstanceUID);
				return StatusCode((int)HttpStatusCode.BadRequest, JsonConvert.SerializeObject(modal));
			}
		}

		[HttpPost("Update")]
		public IActionResult Update()
		{
			var sopInstanceUId = Request.Form["sopInstanceUID"].FirstOrDefault();
			var status = Request.Form["status"].FirstOrDefault();
			var _model = da.UpdateStatus(sopInstanceUId, status);
			if (!string.IsNullOrEmpty(_model.DestinationId))
			{
				return StatusCode((int)HttpStatusCode.OK, JsonConvert.SerializeObject(_model));
			}
			else
			{
				da.SaveLogDetails("ERROR", DateTime.Now.ToString() + " No destination found for the modality", sopInstanceUId);
				return StatusCode((int)HttpStatusCode.BadRequest, JsonConvert.SerializeObject(_model));
			}
		}

		[HttpPost("SaveLogDetails")]
		public IActionResult SaveLogDetails()
		{
			var LogLevel = Request.Form["LogLevel"].FirstOrDefault();
			var Exception = Request.Form["Exception"].FirstOrDefault();
			var SOPInstanceUID = Request.Form["sopInstanceUID"].FirstOrDefault();
			da.SaveLogDetails(LogLevel, Exception, SOPInstanceUID);
			return StatusCode((int)HttpStatusCode.OK);
		}
		[HttpPost("UpdateDestinationAppStatus")]
		public IActionResult UpdateDestinationAppStatus()
		{
			var serverDetails = Request.Form["serverDetails"].FirstOrDefault();
			XmlDocument doc = new XmlDocument();
			var destinationId = Request.Form["destinationId"].FirstOrDefault();
			if (serverDetails != null)
			{
				doc = JsonConvert.DeserializeXmlNode(serverDetails, "Root");
			}
			int val = da.UpdateDestinationAppStatus(doc.InnerXml.ToString(),destinationId);
			if (val == -1)
			{
				return StatusCode((int)HttpStatusCode.BadRequest);
			}
			else
			{
				return StatusCode((int)HttpStatusCode.OK);
			}
		}
		[HttpPost("UpdateClientAppStatus")]
		public IActionResult UpdateClientAppStatus()
		{
			var clientId = Request.Form["clientId"].FirstOrDefault();
			int val = da.UpdateClientAppStatus(clientId);
			if (val == -1)
			{
				return StatusCode((int)HttpStatusCode.BadRequest);
			}
			else
			{
				return StatusCode((int)HttpStatusCode.OK);
			}
		}
	}
}
