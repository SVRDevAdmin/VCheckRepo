using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;
using VCheckViewerAPI.Models;

namespace VCheckViewerAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        [HttpGet(Name = "GetTestObject")]
        public ActionResult<IEnumerable<VCheckViewerAPI.Models.TestObject>> GetTestClass()
        {
            //var sTest = new VCheckViewerAPI.Models.TestClass();
            return VCheckViewerAPI.Models.TestClass.GetTestObject();
        }

        [HttpGet(Name = "GetTest1")]
        public ActionResult<VCheckViewerAPI.Message.TestObject.MessageResponse> GetTest1(VCheckViewerAPI.Message.TestObject.MessageRequest sRequest)
        {
            VCheckViewerAPI.Message.TestObject.MessageResponse sResp = new Message.TestObject.MessageResponse();
            sResp.Test = VCheckViewerAPI.Models.TestClass.GetTestObject();

            return sResp;
        }

        [HttpGet(Name = "GetTest12")]
        public ActionResult GetTest2(string test)
        {
            return Ok();
        }

        [HttpPost(Name = "GetPatientResult")]
        public APIResponseModel GetPatientResult([FromHeader] string timestamp, [FromHeader] string clientKey, APIRequestModel request)
        {
            List<TestResultModel> result = new List<TestResultModel>();
            bool ClientAuthenticated = false;
            string responseCode, responseMessage, responseStatus;

            try
            {
                if (request.PatientID != null)
                {
                    using (var ctx = new ClientAuthDBContext())
                    {
                        var ClientAuthIndex = ctx.Mst_Client_Auth.Where(x => x.ClientKey == clientKey).Select(x => x.ClientID).FirstOrDefault();
                        ClientAuthenticated = ctx.Mst_Client.Where(x => x.ID == ClientAuthIndex && x.Status == 1).Any();
                    }

                    if (ClientAuthenticated)
                    {
                        using (var ctx = new TestResultDBContext(Host.CreateApplicationBuilder().Configuration))
                        {
                            result = ctx.txn_testResults.Where(x => x.PatientID == request.PatientID).ToList();

                            if(result.Count > 0) { responseCode = "VV.0001"; responseMessage = "Success"; }
                            else { responseCode = "VV.0002"; responseMessage = "No Data"; }
                            responseStatus = "Success";
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

            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";
            }

            Response.Headers.Append("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Response.Headers.Append("clientKey", clientKey);

            return new APIResponseModel(responseCode, responseStatus, responseMessage, result);
        }

        [HttpPost(Name = "UpdateScheduledTest")]
        public APIResponseModel UpdateScheduledTest([FromHeader] string timestamp, [FromHeader] string clientKey, APIRequestModel request)
        {
            ScheduledTestModel result = new ScheduledTestModel();
            bool ClientAuthenticated = false;
            string responseCode, responseMessage, responseStatus;

            try
            {
                if (request.ScheduledUniqueID != null)
                {
                    using (var ctx = new ClientAuthDBContext())
                    {
                        var ClientAuthIndex = ctx.Mst_Client_Auth.Where(x => x.ClientKey == clientKey).Select(x => x.ClientID).FirstOrDefault();
                        ClientAuthenticated = ctx.Mst_Client.Where(x => x.ID == ClientAuthIndex && x.Status == 1).Any();
                    }

                    if (ClientAuthenticated)
                    {
                        using (var ctx = new ScheduleDBContext(Host.CreateApplicationBuilder().Configuration))
                        {
                            result = ctx.Txn_ScheduledTests.Where(x => x.ScheduleUniqueID == request.ScheduledUniqueID).ToList().FirstOrDefault();

                            if (result != null)
                            {
                                result.ScheduledDateTime = request.ScheduledDatetime != null ? DateTime.Parse(request.ScheduledDatetime) : result.ScheduledDateTime;
                                result.InchargePerson = request.InchargePerson != null ? request.InchargePerson : result.InchargePerson;

                                ctx.Txn_ScheduledTests.Update(result);
                                var tracker1 = ctx.ChangeTracker.DetectChanges;
                                ctx.SaveChanges();
                                var tracker2 = ctx.ChangeTracker.DetectChanges;

                                responseCode = "VV.0001";
                                responseMessage = "Success";
                            }
                            else
                            {
                                responseCode = "VV.0002";
                                responseMessage = "No Data";
                            }

                            responseStatus = "Success";
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
                    responseCode = "VV.2001";
                    responseStatus = "Fail";
                    responseMessage = "Missing Scheduled Unique ID";

                }
                

            }
            catch (Exception ex)
            {
                responseCode = "VV.9999";
                responseStatus = "Fail";
                responseMessage = "Internal Error";
            }

            Response.Headers.Append("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Response.Headers.Append("clientKey", clientKey);

            return new APIResponseModel(responseCode, responseStatus, responseMessage, result);
        }
    }
}
