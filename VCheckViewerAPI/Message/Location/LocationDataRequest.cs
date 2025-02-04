using VCheckViewerAPI.Message.General;

namespace VCheckViewerAPI.Message.Location
{
    public class LocationDataRequest
    {
        public HeaderModel header { get; set; }
        public GetLocationDataRequestBody body { get; set; }
    }

    public class GetLocationDataRequestBody
    {

    }
}
