using VCheck.Lib.Data.Models;
using VCheckViewerAPI.Message.General;
using VCheckViewerAPI.Message.GetPatientResult;

namespace VCheckViewerAPI.Message.GetTestList
{
    public class TestDataResponse
    {
        public HeaderModel Header { get; set; }
        public TestDataResponseBody Body { get; set; }
    }

    public class TestDataResponseBody
    {
        public String? responsecode { get; set; }
        public String? responsestatus { get; set; }
        public String? responsemessage { get; set; }
        public List<TestDataObject>? results { get; set; }
    }

    public class TestDataObject
    {
        public String? testid { get; set; }
        public String? testname { get; set; }
        public String? testdescription { get; set; }
    }
}
