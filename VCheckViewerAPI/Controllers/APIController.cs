using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
