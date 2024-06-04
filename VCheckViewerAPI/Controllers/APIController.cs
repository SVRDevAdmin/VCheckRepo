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
using VCheckViewerAPI.Models;

namespace VCheckViewerAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private ApiRepository _apiRepository = new ApiRepository();

        [HttpGet(Name = "GetTestObject")]
        public ActionResult<IEnumerable<VCheckViewerAPI.Models.TestObject>> GetTestClass()
        {
            throw new NotImplementedException();
            var sTest = new VCheckViewerAPI.Models.TestClass();
            return VCheckViewerAPI.Models.TestClass.GetTestObject();
        }

        [HttpGet(Name = "GetTest1")]
        public ActionResult<VCheckViewerAPI.Message.TestObject.MessageResponse> GetTest1(VCheckViewerAPI.Message.TestObject.MessageRequest sRequest)
        {
            VCheckViewerAPI.Message.TestObject.MessageResponse sResp = new Message.TestObject.MessageResponse();
            sResp.Test = VCheckViewerAPI.Models.TestClass.GetTestObject();

            return sResp;
        }

        [HttpGet(Name = "GetTest2")]
        public ActionResult GetTest2(string test)
        {
            return Ok();
        }

        [HttpPost(Name = "GetPatientResult")]
        public ResponseModel GetPatientResult(PatientDataRequest request)
        {
            throw new NotImplementedException();
            object? result = null;
            string responseCode, responseMessage, responseStatus;

            if (request.Header.ClientKey != null && _apiRepository.Authenticate(request.Header.ClientKey) && request.Body.PatientID != null)
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

            var responseHeader = new HeaderModel() { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ClientKey = request.Header.ClientKey };
            var responseBody = new ResponseBody() { ResponseCode = responseCode, ResponseStatus = responseStatus, ResponseMessage = responseMessage, Results = result };
            var response = new ResponseModel() { Header = responseHeader, Body = responseBody};

            var requestJson = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(request));
            var responseJson = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(response));

            Logger sLogger = new Logger();
            sLogger.ApiLog(requestJson, responseJson);

            return response;
        }

        [HttpPost(Name = "UpdateScheduledTest")]
        public ResponseModel UpdateScheduledTest(ScheduleDataRequest request)
        {
            object? result = null;
            string responseCode, responseMessage, responseStatus;

            if (request.Header.ClientKey != null && _apiRepository.Authenticate(request.Header.ClientKey) && request.Body.ScheduledUniqueID != null)
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

            var responseHeader = new HeaderModel() { Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ClientKey = request.Header.ClientKey };
            var responseBody = new ResponseBody() { ResponseCode = responseCode, ResponseStatus = responseStatus, ResponseMessage = responseMessage, Results = result };
            var response = new ResponseModel() { Header = responseHeader, Body = responseBody };

            Logger sLogger = new Logger();
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
