using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.CodeDom;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using VCheck.Lib.Data;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewerAPI.Lib.Util;
using VCheckViewerAPI.Message.CreateScheduledTest;
using VCheckViewerAPI.Message.General;
using VCheckViewerAPI.Message.GetPatientResult;
using VCheckViewerAPI.Message.GetTestList;
using VCheckViewerAPI.Message.Location;
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
        [HttpPost(Name = "GetPatientResult")]
        public ResponseModel GetPatientResult(PatientDataRequest request)
        {
            var response = new ResponseModel();
            response.Header = new HeaderModel();
            response.Body = new ResponseBody();

           List<List<PatientDataObject>> result = null;
            string responseCode = "";
            String responseMessage = "";
            String responseStatus = "";

            try
            {
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

                var responseHeader = new HeaderModel() { 
                    timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"), 
                    clientKey = request.Header.clientKey 
                };
                var responseBody = new ResponseBody() { 
                    ResponseCode = responseCode, 
                    ResponseStatus = responseStatus, 
                    ResponseMessage = responseMessage, 
                    Results = result 
                };

                response.Header = responseHeader;
                response.Body = responseBody;


                //--------- Log Payload -------//
                VCheck.APILogging.CallLogging.InsertAPiLog("GetPatientResult", Guid.NewGuid().ToString(), request.Header.timestamp,
                                               Newtonsoft.Json.JsonConvert.SerializeObject(request), responseHeader.timestamp,
                                               Newtonsoft.Json.JsonConvert.SerializeObject(response), responseCode, responseStatus,
                                               responseMessage);
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Exception";
                responseMessage = "Exception Error";

                VCheck.APILogging.CallLogging.InsertErrorLog("GetPatientResult", Guid.NewGuid().ToString(), responseCode, responseStatus,
                                            responseMessage, ((ex != null) ? ex.ToString() : ""));
            }

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
            var response = new ResponseModel();
            response.Header = new HeaderModel();
            response.Body = new ResponseBody();
            string responseCode = "";
            String responseMessage = "";
            String responseStatus = "";

            ScheduledTestModel result = null;
            var sResult = new List<ScheduleResultObject>();

            try
            {
                if (!String.IsNullOrEmpty(request.Body.ScheduledUniqueID))
                {
                    if (request.Header.clientKey != null && _apiRepository.Authenticate(request.Header.clientKey))
                    {
                        if (_apiRepository.ValidateTokenExpiry(request.Header.clientKey))
                        {
                            ClientModel sAuthProfile = _apiRepository.GetClientProfileByClientKey(request.Header.clientKey);
                            var UpdatedBy = (sAuthProfile != null) ? sAuthProfile.Name : "";
                            _apiRepository.UpdateScheduledTest(request.Body.ScheduledUniqueID, request.Body.ScheduledDatetime, request.Body.InchargePerson, UpdatedBy,
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

                var responseHeader = new HeaderModel() { 
                    timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"), 
                    clientKey = request.Header.clientKey 
                };
                var responseBody = new ResponseBody() { 
                    ResponseCode = responseCode, 
                    ResponseStatus = responseStatus, 
                    ResponseMessage = responseMessage, 
                    Results = sResult 
                };

                response.Header = responseHeader;
                response.Body = responseBody;

                //--------- Log Payload -------//
                VCheck.APILogging.CallLogging.InsertAPiLog("UpdateScheduledTest", Guid.NewGuid().ToString(), request.Header.timestamp,
                                               Newtonsoft.Json.JsonConvert.SerializeObject(request), responseHeader.timestamp,
                                               Newtonsoft.Json.JsonConvert.SerializeObject(response), responseCode, responseStatus,
                                               responseMessage);
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Exception";
                responseMessage = "Exception Error";

                VCheck.APILogging.CallLogging.InsertErrorLog("UpdateScheduledTest", Guid.NewGuid().ToString(), responseCode, responseStatus,
                            responseMessage, ((ex != null) ? ex.ToString() : ""));
            }

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
                            if (LocationRepository.IsLocationIdExists(ConfigSettings.GetConfigurationSettings(), Convert.ToInt32(request.body.LocationID)))
                            {
                                ClientModel sAuthProifle = _apiRepository.GetClientProfileByClientKey(request.Header.clientKey);

                                ScheduledTestModel scheduledObj = new ScheduledTestModel();
                                scheduledObj.ScheduledTestType = request.body.ScheduledTestName;

                                DateTime dtScheduled = DateTime.MinValue;
                                if (DateTime.TryParseExact(request.body.ScheduledDateTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtScheduled))
                                {
                                    scheduledObj.ScheduledDateTime = dtScheduled;
                                }

                                var uniqueID = request.body.TestUniqueID + "-" + GenerateUniqueKey(8);
                                scheduledObj.ScheduleUniqueID = uniqueID;
                                scheduledObj.ScheduledBy = request.body.ScheduledBy;
                                scheduledObj.InchargePerson = request.body.PersonIncharges;
                                scheduledObj.PatientID = request.body.PatientID;
                                scheduledObj.PatientName = request.body.PatientName;
                                scheduledObj.Gender = request.body.Gender;
                                scheduledObj.Species = request.body.Species;
                                scheduledObj.OwnerName = request.body.OwnerName;
                                scheduledObj.ScheduleTestStatus = 0;
                                scheduledObj.TestCompleted = 0;
                                scheduledObj.CreatedDate = DateTime.Now;
                                scheduledObj.LocationID = Convert.ToInt32(request.body.LocationID);

                                DateTime dtCreated = DateTime.MinValue;
                                if (DateTime.TryParseExact(request.body.ScheduledCreatedDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtCreated))
                                {
                                    scheduledObj.CreatedDate = dtCreated;
                                }
                                else
                                {
                                    scheduledObj.CreatedDate = DateTime.Now;
                                }
                                scheduledObj.CreatedBy = (sAuthProifle != null) ? sAuthProifle.Name : "";

                                //if (ScheduledTestRepository.GetScheduledTestByUniqueID(ConfigSettings.GetConfigurationSettings(),
                                //            request.body.ScheduledUniqueID) == null)
                                //{
                                if (ScheduledTestRepository.InsertScheduledTest(ConfigSettings.GetConfigurationSettings(), scheduledObj))
                                {
                                    sRespCode = "VV.0001";
                                    sRespStatus = "Success";
                                    sRespMessage = "Success. Generated unique ID is " + uniqueID;

                                    //if (request.body.TestUniqueID.Split("-")[0] == "VCHECKC1")
                                    //{
                                    //    HL7MessageSender.Main sender = new HL7MessageSender.Main();
                                    //    sender.SendMessage(request.body);
                                    //}
                                }
                                else
                                {
                                    sRespCode = "VV.2003";
                                    sRespStatus = "Fail";
                                    sRespMessage = "Failed to insert Scheduled Test.";
                                }
                                //}
                                //else
                                //{
                                //    sRespCode = "VV.2004";
                                //    sRespStatus = "Fail";
                                //    sRespMessage = "Duplicate Scheduled Test record found.";
                                //}
                            }
                            else
                            {
                                sRespCode = "VV.1004";
                                sRespStatus = "Fail";
                                sRespMessage = "Invalid Location ID";
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

                sResp.Header.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                sResp.Header.clientKey = request.Header.clientKey;
                sResp.Body.ResponseCode = sRespCode;
                sResp.Body.ResponseStatus = sRespStatus;
                sResp.Body.ResponseMessage = sRespMessage;

                //--------- Log Payload -------//
                VCheck.APILogging.CallLogging.InsertAPiLog("CreateScheduledTest", Guid.NewGuid().ToString(), request.Header.timestamp,
                                                           Newtonsoft.Json.JsonConvert.SerializeObject(request), sResp.Header.timestamp,
                                                           Newtonsoft.Json.JsonConvert.SerializeObject(sResp), sRespCode, sRespStatus, sRespMessage);
            }
            catch (Exception ex)
            {
                sRespCode = "VV.9999";
                sRespStatus = "Exception";
                sRespMessage = "Exception Error";

                VCheck.APILogging.CallLogging.InsertErrorLog("CreateScheduledTest", Guid.NewGuid().ToString(), sRespCode, sRespStatus,
                                                sRespMessage, ((ex != null) ? ex.ToString() : ""));
            }

            return sResp;
        }

        /// <summary>
        /// Get Location List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = "GetLocationList")]
        public ResponseModel GetLocationList(LocationDataRequest request)
        {
            var response = new ResponseModel();
            response.Header = new HeaderModel();
            response.Body = new ResponseBody();

            String responseCode = "";
            String responseMessage = "";
            String responseStatus = "";

            List<LocationResultObject> sResultList = new List<LocationResultObject>();

            try
            {
                if (request.header.clientKey != null && _apiRepository.Authenticate(request.header.clientKey))
                {
                    if (_apiRepository.ValidateTokenExpiry(request.header.clientKey))
                    {
                        var sLocationList = LocationRepository.GetLocationList(ConfigSettings.GetConfigurationSettings());
                        if (sLocationList != null && sLocationList.Count > 0)
                        {
                            foreach(var location in sLocationList)
                            {
                                sResultList.Add(new LocationResultObject
                                {
                                    locationid = location.ID.ToString(),
                                    name = location.Name,
                                    status = location.Status.ToString(),
                                    createddate = (location.CreatedDate != null) ? 
                                                    location.CreatedDate.Value.ToString("yyyyMMddHHmmss") : null,
                                    modifieddate = (location.UpdatedDate != null) ? 
                                                    location.UpdatedDate.Value.ToString("yyyyMMddHHmmss") : null
                                });
                            }
                        }

                        responseCode = "VV.0001";
                        responseStatus = "Success";
                        responseMessage = "Success";
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

                response.Header.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                response.Header.clientKey = request.header.clientKey;

                response.Body.ResponseCode = responseCode;
                response.Body.ResponseStatus = responseStatus;
                response.Body.ResponseMessage = responseMessage;
                response.Body.Results = sResultList;

                //--------- Log Payload -------//
                VCheck.APILogging.CallLogging.InsertAPiLog("GetLocationList", Guid.NewGuid().ToString(), request.header.timestamp,
                                               Newtonsoft.Json.JsonConvert.SerializeObject(request), response.Header.timestamp,
                                               Newtonsoft.Json.JsonConvert.SerializeObject(response), responseCode, responseStatus,
                                               responseMessage);
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Exception";
                responseMessage = "Exception Error";

                VCheck.APILogging.CallLogging.InsertErrorLog("GetLocationList", Guid.NewGuid().ToString(), responseCode, responseStatus,
                                            responseMessage, ((ex != null) ? ex.ToString() : ""));
            }

            return response;
        }

        /// <summary>
        /// Cancel Scheduled Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = "CancelScheduledTest")]
        public ResponseModel CancelScheduledTest(ScheduleDataRequest request)
        {
            var response = new ResponseModel();
            response.Header = new HeaderModel();
            response.Body = new ResponseBody();

            String responseCode = "";
            String responseMessage = "";
            String responseStatus = "";

            try
            {
                if (request.Header.clientKey != null && _apiRepository.Authenticate(request.Header.clientKey))
                {
                    if (_apiRepository.ValidateTokenExpiry(request.Header.clientKey))
                    {
                        if (!String.IsNullOrEmpty(request.Body.ScheduledUniqueID))
                        {
                            ClientModel sAuthProfile = _apiRepository.GetClientProfileByClientKey(request.Header.clientKey);
                            var UpdatedBy = (sAuthProfile != null) ? sAuthProfile.Name : "";

                            _apiRepository.CancelScheduledTest(request.Body.ScheduledUniqueID, UpdatedBy, out responseCode, out responseMessage, out responseStatus);
                        }
                        else
                        {
                            responseCode = "VV.2002";
                            responseStatus = "Fail";
                            responseMessage = "Missing Scheduled Unique ID";
                        }

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

                response.Header.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                response.Header.clientKey = request.Header.clientKey;

                response.Body.ResponseCode = responseCode;
                response.Body.ResponseStatus = responseStatus;
                response.Body.ResponseMessage = responseMessage;

                //--------- Log Payload -------//
                VCheck.APILogging.CallLogging.InsertAPiLog("CancelScheduledTest", Guid.NewGuid().ToString(), request.Header.timestamp,
                                               Newtonsoft.Json.JsonConvert.SerializeObject(request), response.Header.timestamp,
                                               Newtonsoft.Json.JsonConvert.SerializeObject(response), responseCode, responseStatus,
                                               responseMessage);
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Exception";
                responseMessage = "Exception Error";

                VCheck.APILogging.CallLogging.InsertErrorLog("CancelScheduledTest", Guid.NewGuid().ToString(), responseCode, responseStatus,
                                            responseMessage, ((ex != null) ? ex.ToString() : ""));
            }

            return response;
        }

        /// <summary>
        /// Test connection
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(Name = "TestConnection")]
        public ResponseModel TestConnection()
        {
            var response = new ResponseModel();
            response.Header = new HeaderModel();
            response.Body = new ResponseBody();

            response.Header.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            response.Header.clientKey = "";

            response.Body.ResponseCode = "VV.0001";
            response.Body.ResponseStatus = "Success";
            response.Body.ResponseMessage = "Success";
            response.Body.Results = null;

            return response;
        }


        //[HttpGet(Name = "TestSQLite")]
        //public void TestSQLite()
        //{
        //    using (var connection = new SqliteConnection("Data Source=C:\\Users\\azwan\\Downloads\\sqlite-tools-win-x64-3460000\\Databases\\vcheck.db"))
        //    {
        //        connection.Open();

        //        var command = connection.CreateCommand();

        //        //command.CommandText = "Insert into apiLog values('test','test')";

        //        //command.ExecuteReader();

        //        //command.Dispose();

        //        command.CommandText =
        //        @"
        //            SELECT *
        //            FROM apiLog
        //        ";

        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                var name = reader.GetString(0);

        //                Console.WriteLine($"Hello, {name}!");
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Get Test List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = "GetTestList")]
        public ResponseModel GetTestList(TestDataRequest request)
        {
            var response = new ResponseModel();
            response.Header = new HeaderModel();
            response.Body = new ResponseBody();

            String responseCode = "";
            String responseMessage = "";
            String responseStatus = "";

            List<TestDataObject> sResultList = new List<TestDataObject>();

            try
            {
                if (request.header.clientKey != null && _apiRepository.Authenticate(request.header.clientKey))
                {
                    if (_apiRepository.ValidateTokenExpiry(request.header.clientKey))
                    {
                        var sTestList = TestResultsRepository.GetAllTestList(ConfigSettings.GetConfigurationSettings());
                        if (sTestList != null && sTestList.Count > 0)
                        {
                            foreach (var test in sTestList)
                            {
                                sResultList.Add(new TestDataObject
                                {
                                    testid = test.TestID, 
                                    testname = test.TestName, 
                                    testdescription = test.TestDescription
                                });
                            }
                        }

                        responseCode = "VV.0001";
                        responseStatus = "Success";
                        responseMessage = "Success";
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

                response.Header.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                response.Header.clientKey = request.header.clientKey;

                response.Body.ResponseCode = responseCode;
                response.Body.ResponseStatus = responseStatus;
                response.Body.ResponseMessage = responseMessage;
                response.Body.Results = sResultList;

                //--------- Log Payload -------//
                VCheck.APILogging.CallLogging.InsertAPiLog("GetTestList", Guid.NewGuid().ToString(), request.header.timestamp,
                                               Newtonsoft.Json.JsonConvert.SerializeObject(request), response.Header.timestamp,
                                               Newtonsoft.Json.JsonConvert.SerializeObject(response), responseCode, responseStatus,
                                               responseMessage);
            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Exception";
                responseMessage = "Exception Error";

                VCheck.APILogging.CallLogging.InsertErrorLog("GetTestList", Guid.NewGuid().ToString(), responseCode, responseStatus,
                                            responseMessage, ((ex != null) ? ex.ToString() : ""));
            }

            return response;
        }

        public static string GenerateUniqueKey(int size)
        {
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray(); ;
            byte[] data = new byte[4 * size];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();

            //return Guid.NewGuid().ToString("n").Substring(0, size);

            //Random random = new Random();

            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //return new string(Enumerable.Repeat(chars, size)
            //    .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
