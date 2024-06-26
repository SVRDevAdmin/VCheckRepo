﻿using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Get Patient Result API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetPatientResult")]
        public ResponseModel GetPatientResult(PatientDataRequest request)
        {
            List<PatientDataObject> result = null;
            string responseCode, responseMessage, responseStatus;

            if (request.Header.clientKey != null && _apiRepository.Authenticate(request.Header.clientKey) && request.Body.PatientID != null)
            {
                _apiRepository.GetTestResults(request.Body.PatientID, out result, out responseCode, out responseMessage, out responseStatus);
            }
            else if (request.Body.PatientID == null)
            {
                responseCode = "VV.1002";
                responseStatus = "Fail";
                responseMessage = "Missing Patient ID";
            }
            else
            {
                responseCode = "VV.0003";
                responseStatus = "Fail";
                responseMessage = "Unauthorized Request";
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
            string responseCode, responseMessage, responseStatus;

            if (request.Header.clientKey != null && _apiRepository.Authenticate(request.Header.clientKey) && request.Body.ScheduledUniqueID != null)
            {
                _apiRepository.UpdateScheduledTest(request.Body.ScheduledUniqueID, request.Body.ScheduledDatetime, request.Body.InchargePerson, out result, out responseCode, out responseMessage, out responseStatus);
            }
            else if (request.Body.ScheduledUniqueID == null)
            {
                responseCode = "VV.2002";
                responseStatus = "Fail";
                responseMessage = "Missing Scheduled Unique ID";
            }
            else
            {
                responseCode = "VV.0003";
                responseStatus = "Fail";
                responseMessage = "Unauthorized Request";
            }

            var sResult = new List<ScheduleResultObject>();
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

            var responseHeader = new HeaderModel() { timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"), clientKey = request.Header.clientKey };
            var responseBody = new ResponseBody() { ResponseCode = responseCode, ResponseStatus = responseStatus, ResponseMessage = responseMessage, Results = sResult };
            var response = new ResponseModel() { Header = responseHeader, Body = responseBody };

            APILogging sLogger = new APILogging();
            sLogger.ApiLog(request, response);

            return response;
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
