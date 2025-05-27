using VCheckViewerAPI.Message.CreateScheduledTest;
using VCheckViewerAPI.Message.General;

namespace VCheckViewerAPI.Message.Location
{
    public class UpdateLocationDataRequest
    {
        public HeaderModel Header { get; set; }
        public UpdateLocationDataRequestBody Body { get; set; }
    }

    public class UpdateLocationDataRequestBody
    {
        public String? ID { get; set; }
        public String? Name { get; set; }
        public String? Address { get; set; }
        public String? ContactName { get; set; }
        public String? PhoneNum { get; set; }
        public String? Email { get; set; }
        public String? Description { get; set; }
        public int? Status { get; set; }
        public String? CreatedBy { get; set; }
    }
}
