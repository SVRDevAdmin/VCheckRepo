using log4net;
using Microsoft.Extensions.Hosting;
using Mysqlx.Crud;
using Mysqlx.Session;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewerAPI.Lib.Util;

namespace VCheck.Lib.Data
{
    public class ApiRepository
    {
        private ClientAuthDBContext _AuthContext = new ClientAuthDBContext();
        //private ScheduleDBContext _scheduleTestContext = new ScheduleDBContext(Host.CreateApplicationBuilder().Configuration);
        //private TestResultDBContext _testResultContext = new TestResultDBContext(Host.CreateApplicationBuilder().Configuration);

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        public bool Authenticate(string clientKey, out int CanViewOther)
        {
            CanViewOther = 0;

            try
            {
                var ClientAuthIndex = _AuthContext.Mst_Client_Auth.Where(x => x.ClientKey == clientKey).FirstOrDefault();
                CanViewOther = ClientAuthIndex.CanViewOthers;

                return _AuthContext.Mst_Client.Where(x => x.ID == ClientAuthIndex.ID && x.Status == 1).Any();
            }
            catch (Exception ex)
            {
                log.Error("Database Error >>> ", ex);
                return false;
            }
        }

        public bool ValidateTokenExpiry(String clientkey)
        {
            DateOnly sToday = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                return _AuthContext.Mst_Client_Auth.Where(x => x.ClientKey == clientkey && (x.StartDate <= sToday && x.EndDate >= sToday)).Any();
            }
            catch (Exception ex)
            {
                log.Error("ApiRepository >>> ValidateTokenExpiry >>> " + ex.ToString());
                return false;
            }
        }

        public ClientModel GetClientProfileByClientKey(String sClientKey)
        {
            try
            {
                var sClientAuth = _AuthContext.Mst_Client_Auth.Where(x => x.ClientKey == sClientKey).FirstOrDefault();
                if (sClientAuth != null)
                {
                    return _AuthContext.Mst_Client.Where(x => x.ID == sClientAuth.ClientID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error("ApiRepository >>> GetClientProfileByClientKey >>> " + ex.ToString());
            }

            return null;
        }

        public void GetTestResults(string patientID, out List<List<PatientDataObject>> returnData, out string responseCode, out string responseMessage, out string responseStatus)
        {
            returnData = null;

            try
            {
                var data = TestResultsRepository.GetTestResultByPatientID(ConfigSettings.GetConfigurationSettings(), patientID);

                var testDatetime = data.Select(x => x.observationdatetime).Distinct().ToList();

                List<List<PatientDataObject>> testResultHistory = new List<List<PatientDataObject>>();

                foreach (var datetime in testDatetime)
                {
                    testResultHistory.Add(data.Where(x => x.observationdatetime == datetime).ToList());
                }


                if (data.Count > 0) { 
                    responseCode = "VV.0001"; 
                    responseMessage = "Success"; 
                    returnData = testResultHistory; 
                }
                else { 
                    responseCode = "VV.0002"; 
                    responseMessage = "No Data Found"; 
                }
                responseStatus = "Success";
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";

                log.Error("Database Error >>> ", ex);
            }
        }

        public void UpdateScheduledTest(string scheduledUniqueID ,string scheduledDatetime, string inchargePerson, string analyzerName, string updatedBy, out ScheduledTestModel returnData, out string responseCode, out string responseMessage, out string responseStatus)
        {
            returnData = null;

            try
            {
                var data = ScheduledTestRepository.GetScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(), scheduledUniqueID);
                if (data == null)
                {
                    responseCode = "VV.0002";
                    responseMessage = "No Data Found";
                }
                else if ((!String.IsNullOrEmpty(scheduledDatetime) && data.ScheduledDateTime != DateTime.ParseExact(scheduledDatetime, "yyyyMMddHHmmss", null)) || (!String.IsNullOrEmpty(inchargePerson) && data.InchargePerson != inchargePerson))
                {
                    data.ScheduledDateTime = (!String.IsNullOrEmpty(scheduledDatetime)) ? DateTime.ParseExact(scheduledDatetime, "yyyyMMddHHmmss", null) : data.ScheduledDateTime;
                    data.InchargePerson = (!String.IsNullOrEmpty(inchargePerson)) ? inchargePerson : data.InchargePerson;
                    data.UpdatedBy = updatedBy;
                    data.UpdatedDate = DateTime.Now;

                    ScheduledTestRepository.UpdateScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(), data);

                    responseCode = "VV.0001";
                    responseMessage = "Success";
                    returnData = data;
                }
                else if (!string.IsNullOrEmpty(analyzerName) && analyzerName != data.SentToAnalyzer)
                {
                    data.SentToAnalyzer = analyzerName;
                    data.UpdatedBy = updatedBy;
                    data.UpdatedDate = DateTime.Now;

                    ScheduledTestRepository.UpdateScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(), data);

                    responseCode = "VV.0001";
                    responseMessage = "Success";
                    returnData = data;
                }
                else
                {
                    responseCode = "VV.2001";
                    responseMessage = "No Changes Detected";
                    returnData = data;
                }

                responseStatus = "Success";
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";

                log.Error("Database Error >>> ", ex);
            }
        }

        public void GetScheduleListByLocation(string locationID, out List<ScheduledTestModel> returnData, out string responseCode, out string responseMessage, out string responseStatus)
        {
            returnData = null;

            try
            {
                var data = ScheduledTestRepository.GetScheduleListByLocation(ConfigSettings.GetConfigurationSettings(), locationID);
                if (data == null)
                {
                    responseCode = "VV.0002";
                    responseMessage = "No Data Found";
                }
                else
                {
                    responseCode = "VV.0001";
                    responseMessage = "Success";
                    returnData = data;
                }

                responseStatus = "Success";
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";

                log.Error("Database Error >>> ", ex);
            }
        }

        public void GetScheduleListByLocationNotSent(string locationID, string uniqueID, out List<ScheduledTestModelExtended> returnData, out string responseCode, out string responseMessage, out string responseStatus)
        {
            returnData = null;

            try
            {
                var data = ScheduledTestRepository.GetScheduleListByLocationNotSent(ConfigSettings.GetConfigurationSettings(), locationID, uniqueID);
                if (data == null)
                {
                    responseCode = "VV.0002";
                    responseMessage = "No Data Found";
                }
                else
                {
                    responseCode = "VV.0001";
                    responseMessage = "Success";
                    returnData = data;
                }

                responseStatus = "Success";
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";

                log.Error("Database Error >>> ", ex);
            }
        }

        //public void GetScheduleByLocationPatientID(string locationID, string patientID, List<string> parameters, out ScheduledTestModel returnData, out string responseCode, out string responseMessage, out string responseStatus)
        public void GetScheduleByLocationPatientID(string locationID, string patientID, string testName, out ScheduledTestModel returnData, out string responseCode, out string responseMessage, out string responseStatus)
        {
            returnData = null;

            try
            {
                //var testTypeList = TestResultsRepository.GetAllTestListByParameters(ConfigSettings.GetConfigurationSettings(), parameters);
                //var data = ScheduledTestRepository.GetScheduleByLocationPatientID(ConfigSettings.GetConfigurationSettings(), locationID, patientID, testTypeList.ToArray());
                var data = ScheduledTestRepository.GetScheduleByLocationPatientID(ConfigSettings.GetConfigurationSettings(), locationID, patientID, testName);

                if (data == null)
                {
                    responseCode = "VV.0002";
                    responseMessage = "No Data Found";
                }
                else
                {
                    responseCode = "VV.0001";
                    responseMessage = "Success";
                    returnData = data;
                }

                responseStatus = "Success";
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";

                log.Error("Database Error >>> ", ex);
            }
        }

        public void GetScheduleByUniqueID(string scheduledUniqueID, out ScheduledTestModel returnData, out string responseCode, out string responseMessage, out string responseStatus)
        {
            returnData = null;

            try
            {
                var data = ScheduledTestRepository.GetScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(), scheduledUniqueID, "Unique");
                if (data == null)
                {
                    responseCode = "VV.0002";
                    responseMessage = "No Data Found";
                }
                else
                {
                    responseCode = "VV.0001";
                    responseMessage = "Success";
                    returnData = data;
                }

                responseStatus = "Success";
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";

                log.Error("Database Error >>> ", ex);
            }
        }

        public void CancelScheduledTestByUniqueID(string scheduledUniqueID, string updatedBy, out string responseCode, out string responseMessage, out string responseStatus)
        {
            try
            {
                var data = ScheduledTestRepository.GetScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(), scheduledUniqueID);
                if (data == null)
                {
                    responseCode = "VV.0002";
                    responseMessage = "No Data Found";
                }
                else if (data.ScheduleTestStatus == 1)
                {
                    responseCode = "VV.0001";
                    responseMessage = "Scheduled test are already canceled.";
                }
                else
                {
                    data.ScheduleTestStatus = 1;
                    data.UpdatedBy = updatedBy;
                    data.UpdatedDate = DateTime.Now.ToUniversalTime();

                    ScheduledTestRepository.UpdateScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(), data);

                    responseCode = "VV.0001";
                    responseMessage = "Scheduled test successfully canceled.";
                }

                responseStatus = "Success";
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";

                log.Error("Database Error >>> ", ex);
            }
        }

        public void CancelScheduledTestByOrderID(string orderID, string locationID, string updatedBy, out string responseCode, out string responseMessage, out string responseStatus)
        {
            try
            {
                var data = ScheduledTestRepository.GetScheduledTestByOrderID(ConfigSettings.GetConfigurationSettings(), orderID, updatedBy, locationID);
                if (data == null)
                {
                    responseCode = "VV.0002";
                    responseMessage = "No Data Found";
                }
                else if (data.ScheduleTestStatus == 2)
                {
                    responseCode = "VV.0001";
                    responseMessage = "Scheduled test are already canceled.";
                }
                else
                {
                    data.ScheduleTestStatus = 2;
                    data.UpdatedBy = updatedBy;
                    data.UpdatedDate = DateTime.Now.ToUniversalTime();

                    ScheduledTestRepository.UpdateScheduledTestStatus(ConfigSettings.GetConfigurationSettings(), data, updatedBy);
                    //ScheduledTestRepository.UpdateScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(), data);

                    responseCode = "VV.0001";
                    responseMessage = "Scheduled test successfully canceled.";
                }

                responseStatus = "Success";
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";

                log.Error("Database Error >>> ", ex);
            }
        }

        public void CloseScheduledTestByOrderID(string orderID, string locationID, string updatedBy, out string responseCode, out string responseMessage, out string responseStatus)
        {
            try
            {
                var data = ScheduledTestRepository.GetScheduledTestByOrderID(ConfigSettings.GetConfigurationSettings(), orderID, updatedBy, locationID);
                if (data == null)
                {
                    responseCode = "VV.0002";
                    responseMessage = "No Data Found";
                }
                else if (data.ScheduleTestStatus == 3)
                {
                    responseCode = "VV.0001";
                    responseMessage = "Scheduled test are already closed.";
                }
                else
                {
                    data.ScheduleTestStatus = 3;
                    data.TestCompleted = 1;
                    data.UpdatedBy = updatedBy;
                    data.UpdatedDate = DateTime.Now.ToUniversalTime();

                    ScheduledTestRepository.UpdateScheduledTestStatus(ConfigSettings.GetConfigurationSettings(), data, updatedBy);

                    responseCode = "VV.0001";
                    responseMessage = "Scheduled test successfully canceled.";
                }

                responseStatus = "Success";
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";

                log.Error("Database Error >>> ", ex);
            }
        }

        public void SentScheduledTestByOrderID(string orderID, string locationID, string updatedBy, out string responseCode, out string responseMessage, out string responseStatus)
        {
            try
            {
                var data = ScheduledTestRepository.GetScheduledTestByOrderID(ConfigSettings.GetConfigurationSettings(), orderID, updatedBy, locationID);
                if (data == null)
                {
                    responseCode = "VV.0002";
                    responseMessage = "No Data Found";
                }
                else if (data.ScheduleTestStatus == 1)
                {
                    responseCode = "VV.0001";
                    responseMessage = "Scheduled test are already sent.";
                }
                else
                {
                    data.ScheduleTestStatus = 1;
                    data.UpdatedBy = updatedBy;
                    data.UpdatedDate = DateTime.Now.ToUniversalTime();

                    ScheduledTestRepository.UpdateScheduledTestStatus(ConfigSettings.GetConfigurationSettings(), data, updatedBy);

                    responseCode = "VV.0001";
                    responseMessage = "Scheduled test successfully canceled.";
                }

                responseStatus = "Success";
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";

                log.Error("Database Error >>> ", ex);
            }
        }

        public bool ValidateTestInfo(string TestUniqueID, string Species, string Gender, out int isMismatchedWrongUniqueIDError, out string TestName)
        {
            isMismatchedWrongUniqueIDError = 0;
            var uniqueKey = TestUniqueID.Split("-");
            TestName = "";

            try
            {
                var data = TestResultsRepository.GetTestByUniqueID(ConfigSettings.GetConfigurationSettings(), uniqueKey[0]);
                if (data == null)
                {
                    return false;
                }
                else if (!data.Species.ToLower().Split(",").Contains(Species.ToLower()) || !data.Gender.ToLower().Split(",").Contains(Gender.ToLower()))
                {
                    isMismatchedWrongUniqueIDError = 1;
                    return false;
                }
                else if (uniqueKey.Length == 1)
                {
                    isMismatchedWrongUniqueIDError = 3;
                    return false;
                }
                else
                {
                    TestName = data.TestName;
                    return true;
                }
            }
            catch (Exception ex)
            {

                log.Error("Database Error >>> ", ex);

                isMismatchedWrongUniqueIDError = 2;
                return false;
            }
        }

        public int GetAccessionNo(string clientName, string orderID)
        {
            var accessionNo = 0;
            try
            {
                var client = _AuthContext.Mst_Client.FirstOrDefault(x => x.Name == clientName);
                accessionNo = client.RunningNo;
                client.RunningNo = accessionNo + 1;

                _AuthContext.Update(client);
                _AuthContext.SaveChanges();

                return accessionNo;
            }
            catch (Exception ex)
            {

                log.Error("Database Error >>> ", ex);
                return 0;
            }
        }

        public string GetURLByClientID(int clientID)
        {
            try
            {
                var client = _AuthContext.Mst_URL.FirstOrDefault(x => x.ClientID == clientID);
                var url = client != null ? client.URL : "";
                var port = client != null && client.Port != "80" ? ":" + client.Port : "";
                return client != null ? url + port : "No URL found.";
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
