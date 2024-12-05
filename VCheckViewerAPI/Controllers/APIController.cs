using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using VCheck.Lib.Data;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewerAPI.Lib.Util;
using VCheckViewerAPI.Message.CreateScheduledTest;
using VCheckViewerAPI.Message.General;
using VCheckViewerAPI.Message.GetPatientResult;
using VCheckViewerAPI.Message.UpdateScheduledTest;

namespace VCheckViewerAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private ApiRepository _apiRepository = new ApiRepository();
        private APILogging sLogger = new APILogging();

        /// <summary>
        /// Get Patient Result API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetPatientResult")]
        public ResponseModel GetPatientResult(PatientDataRequest request)
        {
            List<PatientDataObject> result = null;
            string responseCode = "";
            String responseMessage = "";
            String responseStatus = "";

            if (!String.IsNullOrEmpty(request.Body.PatientID))
            {
                if (request.Header.clientKey != null && _apiRepository.Authenticate(request.Header.clientKey))
                {
                    if (_apiRepository.ValidateTokenExpiry(request.Header.clientKey))
                    {
                        _apiRepository.GetTestResults(request.Body.PatientID, out result, out responseCode, out responseMessage, out responseStatus);
                    }
                    else
                    {
                        responseCode = "VV.0005";
                        responseStatus = "Fail";
                        responseMessage = "Expiry Token Key";
                    }
                }
                else
                {
                    responseCode = "VV.0003";
                    responseStatus = "Fail";
                    responseMessage = "Unauthorized Request";
                }
            }
            else
            {
                responseCode = "VV.1002";
                responseStatus = "Fail";
                responseMessage = "Missing Patient ID";
            }

            var responseHeader = new HeaderModel() { timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"), clientKey = request.Header.clientKey };
            var responseBody = new ResponseBody() { ResponseCode = responseCode, ResponseStatus = responseStatus, ResponseMessage = responseMessage, Results = result };
            var response = new ResponseModel() { Header = responseHeader, Body = responseBody};

            var requestJson = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(request));
            var responseJson = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(response));

            APILogging sLogger = new APILogging();
            sLogger.ApiLog(requestJson, responseJson);

            return response;
        }

        /// <summary>
        /// Update Scheduled Test Info API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = "UpdateScheduledTest")]
        public ResponseModel UpdateScheduledTest(ScheduleDataRequest request)
        {
            ScheduledTestModel result = null;
            string responseCode = "";
            String responseMessage = "";
            String responseStatus = "";

            var sResult = new List<ScheduleResultObject>();
            if (!String.IsNullOrEmpty(request.Body.ScheduledUniqueID))
            {
                if (request.Header.clientKey != null && _apiRepository.Authenticate(request.Header.clientKey))
                {
                    if (_apiRepository.ValidateTokenExpiry(request.Header.clientKey))
                    {
                        _apiRepository.UpdateScheduledTest(request.Body.ScheduledUniqueID, request.Body.ScheduledDatetime, request.Body.InchargePerson,
                                                        out result, out responseCode, out responseMessage, out responseStatus);

                        if (result != null)
                        {
                            sResult.Add(new ScheduleResultObject
                            {
                                scheduleduniqueid = result.ScheduleUniqueID,
                                scheduleddatetime = (result.ScheduledDateTime != null) ?
                                                        result.ScheduledDateTime.Value.ToString("yyyyMMddHHmmss") : null,
                                scheduledtesttype = result.ScheduledTestType,
                                scheduledby = result.ScheduledBy,
                                inchargeperson = result.InchargePerson,
                                patientid = result.PatientID
                            });
                        };
                    }
                    else
                    {
                        responseCode = "VV.0005";
                        responseStatus = "Fail";
                        responseMessage = "Expiry Token Key";
                    }
                }
                else
                {
                    responseCode = "VV.0003";
                    responseStatus = "Fail";
                    responseMessage = "Unauthorized Request";
                }
            }
            else
            {
                responseCode = "VV.2002";
                responseStatus = "Fail";
                responseMessage = "Missing Scheduled Unique ID";
            }

            var responseHeader = new HeaderModel() { timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"), clientKey = request.Header.clientKey };
            var responseBody = new ResponseBody() { ResponseCode = responseCode, ResponseStatus = responseStatus, ResponseMessage = responseMessage, Results = sResult };
            var response = new ResponseModel() { Header = responseHeader, Body = responseBody };

            APILogging sLogger = new APILogging();
            sLogger.ApiLog(request, response);

            return response;
        }

        /// <summary>
        /// API for Insert Scheduled Test
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = "CreateScheduledTest")]
        public ResponseModel CreateScheduledTest(CreateScheduledDataRequest request)
        {
            var sResp = new Message.General.ResponseModel();
            sResp.Header = new Message.General.HeaderModel();
            sResp.Body = new Message.General.ResponseBody();

            String sRespCode = "";
            String sRespStatus = "";
            String sRespMessage = "";

            try
            {
                if (request.body.ValidateMandatoryField())
                {
                    if (_apiRepository.Authenticate(request.Header.clientKey))
                    {
                        if (_apiRepository.ValidateTokenExpiry(request.Header.clientKey))
                        {
                            ClientModel sAuthProifle = _apiRepository.GetClientProfileByClientKey(request.Header.clientKey);

                            ScheduledTestModel scheduledObj = new ScheduledTestModel();
                            scheduledObj.ScheduledTestType = request.body.ScheduledTestName;

                            DateTime dtScheduled = DateTime.MinValue;
                            if (DateTime.TryParseExact(request.body.ScheduledDateTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtScheduled))
                            {
                                scheduledObj.ScheduledDateTime = dtScheduled;
                            }

                            scheduledObj.ScheduleUniqueID = request.body.ScheduledUniqueID;
                            scheduledObj.ScheduledBy = request.body.ScheduledBy;
                            scheduledObj.InchargePerson = request.body.PersonIncharges;
                            scheduledObj.PatientID = request.body.PatientID;
                            scheduledObj.PatientName = request.body.PatientName;
                            scheduledObj.Gender = request.body.Gender;
                            scheduledObj.Species = request.body.Species;
                            scheduledObj.OwnerName = request.body.OwnerName;
                            scheduledObj.ScheduleTestStatus = 0;
                            scheduledObj.TestCompleted = 0;

                            DateTime dtCreated = DateTime.MinValue;
                            if (DateTime.TryParseExact(request.body.ScheduledCreatedDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtCreated))
                            {
                                scheduledObj.CreatedDate = dtCreated;
                            }
                            scheduledObj.CreatedBy = (sAuthProifle != null) ? sAuthProifle.Name : "";

                            if (ScheduledTestRepository.GetScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(),
                                        request.body.ScheduledUniqueID) == null)
                            {
                                if (ScheduledTestRepository.InsertScheduledTest(ConfigSettings.GetConfigurationSettings(), scheduledObj))
                                {
                                    sRespCode = "VV.0001";
                                    sRespStatus = "Success";
                                    sRespMessage = "Success";
                                }
                                else
                                {
                                    sRespCode = "VV.2003";
                                    sRespStatus = "Fail";
                                    sRespMessage = "Failed to insert Scheduled Test.";
                                }
                            }
                            else
                            {
                                sRespCode = "VV.2004";
                                sRespStatus = "Fail";
                                sRespMessage = "Duplicate Scheduled Test record found.";
                            }
                        }
                        else
                        {
                            sRespCode = "VV.0005";
                            sRespStatus = "Fail";
                            sRespMessage = "Expiry Token Key";
                        }
                    }
                    else
                    {
                        sRespCode = "VV.0003";
                        sRespStatus = "Fail";
                        sRespMessage = "Unauthorized Request";
                    }
                }
                else
                {
                    sRespCode = "VV.0004";
                    sRespStatus = "Fail";
                    sRespMessage = "Missing Mandatory Fields";
                }
            }
            catch (Exception ex)
            {
                sRespCode = "VV.9999";
                sRespStatus = "Exception";
                sRespMessage = "Exception Error";
            }

            sResp.Header.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            sResp.Header.clientKey = request.Header.clientKey;
            sResp.Body.ResponseCode = sRespCode;
            sResp.Body.ResponseStatus = sRespStatus;
            sResp.Body.ResponseMessage = sRespMessage;

            sLogger.ApiLog(request, sResp);

            return sResp;
        }


        [HttpGet(Name = "TestSQLite")]
        public void TestSQLite()
        {
            using (var connection = new SqliteConnection("Data Source=C:\\Users\\azwan\\Downloads\\sqlite-tools-win-x64-3460000\\Databases\\vcheck.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();

                //command.CommandText = "Insert into apiLog values('test','test')";

                //command.ExecuteReader();

                //command.Dispose();

                command.CommandText =
                @"
                    SELECT *
                    FROM apiLog
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader.GetString(0);

                        Console.WriteLine($"Hello, {name}!");
                    }
                }
            }
        }
    }
}
