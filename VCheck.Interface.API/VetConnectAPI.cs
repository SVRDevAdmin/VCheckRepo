using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using VCheck.Interface.API.General;

namespace VCheck.Interface.API
{
    /// <summary>
    /// interface communicate with VetConnect 
    /// </summary>
    public class VetConnectAPI
    {
        public static IConfiguration iConfig;

        public VetConnectAPI()
        {
            var sBuilder = new ConfigurationBuilder();
            sBuilder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            iConfig = sBuilder.Build();
        }

        /// <summary>
        /// Get new appointment / updated appointment by Date Range
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public VetConnect.ResponseMessage.GetAppointmentDateRangeResponse? GetAppointmentByDateRange(VetConnect.RequestMessage.GetAppointmentDateRangeRequest sInput)
        {
            String? sRequestURL = iConfig.GetSection("VetConnect:GetAppointmentByDateRange").Value;

            try
            {
                Dictionary<String, String> sInputQuery = UtilityLib.ToDictionary<String>(sInput.body);

                using (var client = new HttpClient())
                {
                    var req = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(sRequestURL),
                        Content= new StringContent(
                            Newtonsoft.Json.JsonConvert.SerializeObject(sInput), 
                            Encoding.UTF8, "application/json"
                        )
                    };

                    HttpResponseMessage resp = client.SendAsync(req).Result;
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        String strResult = resp.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<VetConnect.ResponseMessage.GetAppointmentDateRangeResponse>(strResult);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
