using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using VCheck.Interface.API.General;

namespace VCheck.Interface.API
{
    public class VetVitalsAPI
    {
        public static IConfiguration iConfig;

        public VetVitalsAPI()
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
        public VetVitals.ResponseMessage.GetAppointmentDateRangeResponse? GetAppointmentByDateRange(VetVitals.RequestMessage.GetAppointmentDateRangeRequest sInput)
        {
            String? sRequestURL = iConfig.GetSection("VetVitals:GetAppointmentByDateRange").Value;

            try
            {
                Dictionary<String, String> sInputQuery = UtilityLib.ToDictionary<String>(sInput.body);

                using (var client = new HttpClient())
                {
                    var req = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(sRequestURL),
                        Content = new StringContent(
                            Newtonsoft.Json.JsonConvert.SerializeObject(sInput),
                            Encoding.UTF8, "application/json"
                        )
                    };

                    HttpResponseMessage resp = client.SendAsync(req).Result;
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        String strResult = resp.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<VetVitals.ResponseMessage.GetAppointmentDateRangeResponse>(strResult);
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

        /// <summary>
        /// Send Test Results 
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public VetVitals.ResponseMessage.UpdateTestResultsResponse? UpdateTestResults(VetVitals.RequestMessage.UpdateTestResultsRequest sInput)
        {
            String? sRequestURL = iConfig.GetSection("VetVitals:UpdateTestResult").Value;

            try
            {
                using (var client = new HttpClient())
                {
                    var req = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(sRequestURL),
                        Content = new StringContent(
                            Newtonsoft.Json.JsonConvert.SerializeObject(sInput),
                            Encoding.UTF8, "application/json"
                        )
                    };

                    HttpResponseMessage resp = client.SendAsync(req).Result;
                    if (resp.IsSuccessStatusCode)
                    {
                        String strResult = resp.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<VetVitals.ResponseMessage.UpdateTestResultsResponse>(strResult);
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
