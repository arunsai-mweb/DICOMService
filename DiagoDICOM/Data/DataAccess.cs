using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DiagoDICOM.Common;
using DiagoDICOM.Models;
namespace DiagoDICOM.Data
{
	public class DataAccess
	{

		public Users CheckIsExistingUser(Users user)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
			{
				{ "UserId", user.UserId },
				{ "UserName", user.UserName },
				{ "EmailId", user.EmailId },
				{ "Password", user.Password },
				{ "IsInitial", user.IsInitial },
				{ "IsChangePassword",user.IsChangePassword }
			};
					var data = connection.QueryFirst<Users>("CheckIsExistingUser", new DynamicParameters(parameters), commandType: CommandType.StoredProcedure);
					return data;
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException == null ? ex.ToString() : ex.InnerException.Message);
				return null;
			}
		}

		public List<Users> GetUsers()
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var data = connection.Query<Users>("GetUsers", CommandType.StoredProcedure).ToList();
					return data;
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}

		public List<Destination> GetDestinations()
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var data = connection.Query<Destination>("GetDestinations", commandType: CommandType.StoredProcedure).ToList();
					return data;
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}
		public List<Modality.LookUpType> GetModalityByClientId(int? clientId)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "ClientId", clientId }
				};
					var data = connection.Query<Modality.LookUpType>("GetModalityByClientId", parameters, commandType: CommandType.StoredProcedure).ToList();
					return data;
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}
		public List<CaseStudies> GetClientCases(int? clientId, int? destinationId = null)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "ClientId", clientId },
					{ "DestinationId", destinationId }
				};
					var data = connection.Query<CaseStudies>("GetClientCases", parameters, commandType: CommandType.StoredProcedure).ToList();
					return data;
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}
		public void SaveDestination(Destination dest, int? clientId)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "DestinationId", dest.DestinationId },
					{ "AETitle", dest.AETitle },
					{ "IpAddress", dest.IpAddress },
					{ "Port", dest.Port },
					{ "IsPushToFolder", dest.IsPushToFolder },
					{ "PushToFolder", dest.PushToFolder },
					{ "PushToPort", dest.PushToPort },
					{ "PushToIpAddress", dest.PushToIpAddress },
					{ "PushToAETitle", dest.PushToAETitle },
					{ "DestinationName", dest.DestinationName },
					{ "Id",dest.Id},
					{ "Mode", dest.Id == null ? "Add" : "Edit"}
				};
					connection.Query<Destination>("AddEditDestinations", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}

		public Destination GetDestinationById(int? id)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "Id", id }
				};
					var data = connection.QueryFirst<Destination>("GetDestinationById", parameters, commandType: CommandType.StoredProcedure);
					return data;
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				return new Destination();
			}
		}
		public void DeleteDestination(int? id)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "Id", id }
				};
					connection.Query("DeleteDestination", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}

		public List<Clients> GetClients()
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					return connection.Query<Clients>("GetClients", commandType: CommandType.StoredProcedure).ToList();
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}
		public Clients GetClientById(int? clientId)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "ClientId", clientId }
				};
					var data = connection.QueryFirst<Clients>("GetClientById", parameters, commandType: CommandType.StoredProcedure);
					return data;
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}
		public void AddEditClient(Clients client)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "ClientId", client.ClientId },
					{ "ClientName", client.ClientName}
				};
					connection.Query("AddEditClient", parameters, commandType: CommandType.StoredProcedure);

				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}

		public void AddEditClientRule(Clients client)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "ClientId", client.ClientId },
					{ "DestinationId", client.DestinationId},
					{ "Modality", client.Modality},
					{ "Description", client.Description},
					{ "ClientRuleId", client.ClientRuleId},
				};
					connection.Query("AddEditClientRule", parameters, commandType: CommandType.StoredProcedure);

				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}

		public List<Clients> GetClientRules(int clientId)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "ClientId", clientId }
				};
					return connection.Query<Clients>("GetClientRules", parameters, commandType: CommandType.StoredProcedure).ToList();
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}

		}
		public void DeleteClient(int clientId)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "ClientId", clientId }
				};
					connection.Query("DeleteClient", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}
		public Clients GetClientRuleById(int? clientRuleId)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "ClientRuleId", clientRuleId }
				};
					var data = connection.QueryFirst<Clients>("GetClientRuleById", parameters, commandType: CommandType.StoredProcedure);
					return data;
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}
		public void DeleteClientRule(int clientRuleId)
		{
		  try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "ClientRuleId", clientRuleId }
				};
					connection.Query("DeleteClientRule", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}

		public List<Logs> GetLogs()
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					return connection.Query<Logs>("GetLogs",commandType: CommandType.StoredProcedure).ToList();
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}

		public List<CaseStudies> GetStudyStatus(string studyId, int clientId)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "StudyId", studyId },
					{ "ClientId",clientId}
				};
					return connection.Query<CaseStudies>("GetStudyStatus", parameters, commandType: CommandType.StoredProcedure).ToList();
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}

		public Destination SaveImageStudy(CaseStudies cs)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "PatientId", cs.PatientId },
					{ "PatientName", cs.PatientName},
					{ "ClientId",cs.ClientId},
					{ "Gender",cs.Gender},
					{ "Modality",cs.Modality},
					{ "Age", cs.Age},
					{ "ImageNumber",cs.ImageNumber},
					{ "SeriesNumber",cs.SerialNumber},
					{ "StudyId",cs.StudyId},
					{ "SOPInstanceUID",cs.SOPInstanceUID},
					{ "ScanDate",cs.ScanDate},
					{ "StudyDescription",cs.StudyDescription}
				};
					return connection.QueryFirst<Destination>("SaveImageStudy", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}
		public void SaveLogDetails(string logLevel, string exception, string sopInstanceUID)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "LogLevel", logLevel },
					{ "Exception", exception},
					{ "Value",sopInstanceUID},
				};
					connection.Query("SaveLogDetails", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}

		public Destination UpdateStatus(string SopInstanceUID, string status)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "Status", status },
					{ "SOPInstanceUID", SopInstanceUID},
				};
					return connection.QueryFirst<Destination>("UpdateStatus", parameters, commandType: CommandType.StoredProcedure) ?? new Destination();
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}

		public void DeleteStudies(string studyIds)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "studyIds", studyIds },
				};
					connection.Query("DeleteStudies", parameters, commandType: CommandType.StoredProcedure);
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}

		public int CheckServerStatus(string xml,int destinationId)
		{
			try
			{
				using (MySqlConnection connection = new MySqlConnection(AppUser.ConnectionString))
				{
					var parameters = new Dictionary<string, object>
				{
					{ "xmlData", xml },
					{ "destinationId",destinationId}
				};
					return connection.Query<int>("CheckServerStatus", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
				}
			}
			catch (Exception ex)
			{
				Logs.WriteToLogFile(ex == null ? "No Exception Details" : ex.InnerException.ToString() == null ? ex.ToString() : ex.InnerException.Message);
				throw ex;
			}
		}
	}
}
