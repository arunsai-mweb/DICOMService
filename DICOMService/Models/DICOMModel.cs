using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DICOMService.Common;
using DICOMService.Data;
using Microsoft.Extensions.Configuration;

namespace DICOMService.Models
{
	public class Users
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string EmailId { get; set; }
		public string Password { get; set; }
		public bool IsInitial { get; set; }
		public bool IsChangePassword { get; set; }
		public string ConfirmPassword { get; set; }
		public bool IsForgotPassword { get; set; }
	}

	public class Destination
	{
		public string ClientId { get; set; }
		public string DestinationId { get; set; }
		public string DestinationName { get; set; }
		public int? Port { get; set; }
		public string IpAddress { get; set; }
		public string PushToIpAddress { get; set; }
		public int? PushToPort { get; set; }
		public bool IsPushToFolder { get; set; }
		public string AETitle { get; set; }
		public string PushToAETitle { get; set; }
		public string PushToFolder { get; set; }
		public string Modality { get; set; }
		public string FolderPath { get; set; }
		public string Description { get; set; }
		public int? Id { get; set; }
		public string Mode { get; set; }
		public string Error { get; set; }
		public bool Status { get; set; }
	}

	public class CaseStudies
	{
		public string ClientId { get; set; }
		public string DestinationId { get; set; }
		public string StudyId { get; set; }
		public string PatientName { get; set; }
		public string PatientId { get; set; }
		public string Modality { get; set; }
		public string Gender { get; set; }
		public string Age { get; set; }
		public int Status { get; set; }
		public string DestinationName { get; set; }
		public string ClientName { get; set; }
		public int ImageCount { get; set; }
		public string SOPInstanceUID { get; set; }
		public DateTime CreatedOn { get; set; }
		public string DICOMStudyID { get; set; }
		public string ClientStatus { get; set; }
		public string DestinationStatus { get; set; }
		public string ImageNumber { get; set; }
		public string SerialNumber { get; set; }
		public string ScanDate { get; set; }
		public string ScanTime { get; set; }
		public string StudyDescription { get; set; }
		public int SuccessCount { get; set; }
		public int FailureCount { get; set; }
		public string AccessionNumber { get; set; }
		
	}

	public class Clients : Destination
	{
		public string ClientName { get; set; }
		public int ModalityId { get; set; }
		public int? ClientRuleId { get; set; }
	}

	public class Logs
	{
		public string Value { get; set; }
		public DateTime CreatedOn { get; set; }
		public string ExceptionDetails { get; set; }
		public string LogLevel { get; set; }
		public string StuydId { get; set; }
		public int LogId { get; set; }
		public string ClientId { get; set; }
		public string DestinationId { get; set; }
		public string DestinationName { get; set; }
		public string ClientName { get; set; }
		public static void WriteToLogFile(string text)
		{
			var path = Path.Combine(AppUser.WebRootPath, "Logs", DateTime.Now.Year.ToString(), DateTime.Now.ToShortMonthName());
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			var filePath = Path.Combine(path, DateTime.Now.Date.ToString("dd/MM/yyyy")) + ".txt";

			using (StreamWriter writer = File.AppendText(filePath))
			{
				writer.WriteLine(DateTime.Now.ToString() + " : " + text);
				writer.Close();
			}

			DeleteLogFiles();
		}

		static void DeleteLogFiles()
		{
			var path = Path.Combine(AppUser.WebRootPath, "Logs", (DateTime.Now.Year - 1).ToString());
			if (Directory.Exists(path))
				Directory.Delete(path,true);
		}

	}

	static class DateTimeExtensions
	{
		public static string ToMonthName(this DateTime dateTime)
		{
			return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
		}

		public static string ToShortMonthName(this DateTime dateTime)
		{
			return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
		}
	}

    public class Modality
	{
		//public const short CR = Computed Radiography 
		//public const short CT = Computed Tomography 
		//public const short MR = Magnetic Resonance 
		//public const short NM = Nuclear Medicine 
		//public const short US = Ultrasound 
		//public const short OT = Other 
		//public const short BI = Biomagnetic imaging 
		//public const short CD = Color flow Doppler 
		//public const short DD = Duplex Doppler 
		//public const short DG = Diaphanography 
		//public const short ES = Endoscopy 
		//public const short LS = Laser surface scan 
		//public const short PT = Positron emission tomography (PET) 
		//public const short RG = Radiographic imaging (conventional film/screen) 
		//public const short ST = Single-photon emission computed tomography (SPECT) 
		//public const short TG = Thermography 
		//public const short XA = X-Ray Angiography 
		//public const short RF = Radio Fluoroscopy 
		//public const short RTIMAGE = Radiotherapy Image 
		//public const short RTDOSE = Radiotherapy Dose 
		//public const short RTSTRUCT = Radiotherapy Structure Set 
		//public const short RTPLAN = Radiotherapy Plan 
		//public const short RTRECORD = RT Treatment Record 
		//public const short HC = Hard Copy 
		//public const short DX = Digital Radiography 
		//public const short MG = Mammography 
		//public const short IO = Intra-oral Radiography 
		//public const short PX = Panoramic X-Ray 
		//public const short GM = General Microscopy 
		//public const short SM = Slide Microscopy 
		//public const short XC = External-camera Photography 
		//public const short PR = Presentation State 
		//public const short AU = Audio 
		//public const short ECG = Electrocardiography 
		//public const short EPS = Cardiac Electrophysiology 
		//public const short HD = Hemodynamic Waveform 
		//public const short SR = SR Document 
		//public const short IVUS = Intravascular Ultrasound 
		//public const short OP = Ophthalmic Photography 
		//public const short SMR = Stereometric Relationship 
		//public const short OCT = Optical Coherence Tomography 
		//public const short OPR = Ophthalmic Refraction 
		//public const short OPV = Ophthalmic Visual Field 
		//public const short OPM = Ophthalmic Mapping 
		//public const short KO = Key Object Selection
		//public const short SEG = Segmentation 
		//public const short REG = Registration 

		public const short CR = 1;
		public const short CT = 2;
		public const short MR = 3;
		public const short NM = 4;
		public const short US = 5;
		public const short XR = 6;
		public const short MG = 7;
		public const short PT = 8;
		public const short OT = 9;
		public const short BI = 10;
		public const short CD = 11;
		public const short DD = 12;
		public const short DG = 13;
		public const short ES = 14;
		public const short LS = 15;
		public const short RG = 16;
		public const short ST = 17;
		public const short TG = 18;
		public const short XA = 19;
		public const short RF = 20;
		public const short RTIMAGE = 21;
		public const short RTDOSE = 22;
		public const short RTSTRUCT = 23;
		public const short RTPLAN = 24;
		public const short RTRECORD = 25;
		public const short HC = 26;
		public const short DX = 27;
		public const short IO = 28;
		public const short PX = 29;
		public const short SM = 30;
		public const short XC = 31;
		public const short PR = 32;
		public const short AU = 33;
		public const short ECG = 34;
		public const short EPS = 35;
		public const short HD = 36;
		public const short SR = 37;
		public const short IVUS = 38;
		public const short OP = 39;
		public const short SMR = 40;
		public const short OCT = 41;
		public const short OPR = 42;
		public const short OPV = 43;
		public const short OPM = 44;
		public const short KO = 45;
		public const short SEG = 46;
		public const short REG = 47;
		public const short GM = 48;


		public static List<LookUpType> GetModalityShortNames(string clientId)
		{
			var list = new List<LookUpType>();

			list.Add(new LookUpType { id = Modality.CR, name = "CR" });
			list.Add(new LookUpType { id = Modality.DX, name = "DX" });
			list.Add(new LookUpType { id = Modality.CT, name = "CT" });
			list.Add(new LookUpType { id = Modality.MR, name = "MR" });

			DataAccess da = new DataAccess();
			          da.GetClientRules(clientId);

			return list;
		}

		public class LookUpType
		{
			public int id { get; set; }
			public string name { get; set; }
		}

	}

	public enum Error
	{
		ExistingDestinationId,
		ExistingPort
	}
}
