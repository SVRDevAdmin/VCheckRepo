using VCheck.Lib.Data.Models;

namespace VCheckViewerAPI.Message.GetTestList
{
    public class TestDataRequest
    {
        public HeaderModel Header { get; set; }
        public TestDataRequestBody Body { get; set; }
    }

    public class TestDataRequestBody
    {
        public string? TestCode { get; set; }
        public string? TestName { get; set; }
    }
}
