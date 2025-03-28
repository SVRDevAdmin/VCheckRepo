using VCheckViewerAPI.Message.General;
using VCheckViewerAPI.Message.Location;

namespace VCheckViewerAPI.Message.GetTestList
{
    public class TestDataRequest
    {
        public HeaderModel header { get; set; }
        public TestDataRequestBody body { get; set; }
    }

    public class TestDataRequestBody
    {

    }
}
