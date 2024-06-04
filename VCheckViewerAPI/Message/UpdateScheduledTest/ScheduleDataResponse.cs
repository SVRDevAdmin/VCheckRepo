using VCheckViewerAPI.Message.General;

namespace VCheckViewerAPI.Message.UpdateScheduledTest
{
    public class ScheduleDataResponse
    {
        public HeaderModel Header { get; set; }
        public ScheduleDataResponseBody Body { get; set; }
    }

    public class ScheduleDataResponseBody 
    {
        public String? responsecode { get; set; }
        public String? responsestatus { get; set; }
        public String? responsemessage { get; set; }
        public List<ScheduleResultObject> results { get; set;  }
    }

    public class ScheduleResultObject
    {
        public String? scheduleduniqueid { get; set; }
        public String? scheduledtesttype { get; set; }
        public String? scheduleddatetime { get; set; }
        public String? scheduledby { get; set; }
        public String? patientid { get; set; }
        public String? inchargeperson { get; set; }
    }

}
