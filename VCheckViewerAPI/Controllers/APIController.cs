using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using VCheck.Lib.Data;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
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
        public APIResponseModel GetPatientResult(APIRequestModel requestBody)
        {
            object? response = null;
            string responseCode, responseMessage, responseStatus;

            if (requestBody.PatientID != null)
            {
                _apiRepository.GetTestResults(requestBody.PatientID, out response, out responseCode, out responseMessage, out responseStatus);
            }
            else
            {
                responseCode = "VV.1002";
                responseStatus = "Fail";
                responseMessage = "Missing Patient ID";
            }                              

            return new APIResponseModel(responseCode, responseStatus, responseMessage, response);
        }

        [HttpPost(Name = "UpdateScheduledTest")]
        public APIResponseModel UpdateScheduledTest(APIRequestModel requestBody)
        {
            object? response = null;
            string responseCode, responseMessage, responseStatus;

            if (requestBody.ScheduledUniqueID != null)
            {
                _apiRepository.UpdateScheduledTest(requestBody.ScheduledUniqueID, requestBody.ScheduledDatetime, requestBody.InchargePerson, out response, out responseCode, out responseMessage, out responseStatus);
            }
            else
            {
                responseCode = "VV.2002";
                responseStatus = "Fail";
                responseMessage = "Missing Scheduled Unique ID";

            }

            return new APIResponseModel(responseCode, responseStatus, responseMessage, response);
        }
    }
}
