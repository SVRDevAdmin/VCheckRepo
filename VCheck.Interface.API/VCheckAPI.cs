using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VCheck.Interface.API.Lib.General;

namespace VCheck.Interface.API
{
    public class VCheckAPI
    {
        private string url = "http://vcheckstaging.inteleon.xyz/API";
        //private string url = "http://localhost:82/API";

        private string clientKey = "qwertyuiop123asdfghjkl456zxcvbnm789";


        /// <summary>
        /// Test connection
        /// </summary>
        /// <param name="sResultRequest"></param>
        /// <returns></returns>
        public async Task<bool> TestConnection(string apiURL)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));


                    var resp = await client.GetAsync(apiURL + "/testconnection");
                    if (resp.StatusCode.ToString() == "OK")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Update Location
        /// </summary>
        /// <param name="sResultRequest"></param>
        /// <returns></returns>
        public async Task<string> UpdateLocation(LocationModel location)
        {
            UpdateLocationDataRequest request = new UpdateLocationDataRequest() { Header = new HeaderModel(), Body = new UpdateLocationDataRequestBody()};

            request.Header.timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            request.Header.clientKey = clientKey;

            var temp = JsonConvert.SerializeObject(location);
            request.Body = JsonConvert.DeserializeObject<UpdateLocationDataRequestBody>(temp);

            String strJson = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(strJson, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));


                    HttpResponseMessage resp = await client.PostAsync(url + "/UpdateLocation", content);
                    if (resp.IsSuccessStatusCode)
                    {
                        var resultString = resp.Content.ReadAsStringAsync().Result;
                        var results = JsonConvert.DeserializeObject<ResponseModel>(resultString);
                                                
                        return results.Body.Results != null ? results.Body.Results.ToString() : null;
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
        /// Update Location
        /// </summary>
        /// <param name="sResultRequest"></param>
        /// <returns></returns>
        public async Task<string> GetScheduleList(string locationID)
        {
            ScheduleDataRequest request = new ScheduleDataRequest() { Header = new HeaderModel(), Body = new ScheduleDataRequestBody() };

            request.Header.timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            request.Header.clientKey = clientKey;

            request.Body.LocationID = locationID;

            String strJson = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(strJson, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));


                    HttpResponseMessage resp = await client.PostAsync(url + "/GetScheduleListByLocation", content);
                    if (resp.IsSuccessStatusCode)
                    {
                        var resultString = resp.Content.ReadAsStringAsync().Result;
                        var results = JsonConvert.DeserializeObject<ResponseModel>(resultString);

                        return JsonConvert.SerializeObject(results.Body.Results);
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
        /// Update Location
        /// </summary>
        /// <param name="sResultRequest"></param>
        /// <returns></returns>
        public async Task<string> GetScheduleListNotSent(string locationID)
        {
            ScheduleDataRequest request = new ScheduleDataRequest() { Header = new HeaderModel(), Body = new ScheduleDataRequestBody() };

            request.Header.timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            request.Header.clientKey = clientKey;

            request.Body.LocationID = locationID;

            String strJson = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(strJson, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));


                    HttpResponseMessage resp = await client.PostAsync(url + "/GetScheduleListByLocationNotSent", content);
                    if (resp.IsSuccessStatusCode)
                    {
                        var resultString = resp.Content.ReadAsStringAsync().Result;
                        var results = JsonConvert.DeserializeObject<ResponseModel>(resultString);

                        return JsonConvert.SerializeObject(results.Body.Results);
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
        /// Get Schedule
        /// </summary>
        /// <param name="sResultRequest"></param>
        /// <returns></returns>
        public async Task<string> GetSchedule(string locationID, string patientID, List<string> parameters, string uniqueID = null)
        {
            ScheduleDataRequest request = new ScheduleDataRequest() { Header = new HeaderModel(), Body = new ScheduleDataRequestBody() };

            request.Header.timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            request.Header.clientKey = clientKey;

            request.Body.LocationID = locationID;
            request.Body.PatientID = patientID;
            request.Body.Parameters = parameters;
            request.Body.ScheduledUniqueID = uniqueID;

            String strJson = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(strJson, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));


                    HttpResponseMessage resp = await client.PostAsync(url + "/GetScheduleByLocationPatientID", content);
                    if (resp.IsSuccessStatusCode)
                    {
                        var resultString = resp.Content.ReadAsStringAsync().Result;
                        var results = JsonConvert.DeserializeObject<ResponseModel>(resultString);

                        return JsonConvert.SerializeObject(results.Body.Results);
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
        /// Cancel Schedule
        /// </summary>
        /// <param name="sResultRequest"></param>
        /// <returns></returns>
        public async Task<bool> UpdateScheduleStatus(string locationID, string patientID, string orderID, string clientName, int status)
        {
            ScheduleDataRequest request = new ScheduleDataRequest() { Header = new HeaderModel(), Body = new ScheduleDataRequestBody() };

            var endpoint = "";

            if (status == 1)
            {
                endpoint = "/SentScheduledTest";
            }
            else if(status == 2)
            {
                endpoint = "/CancelScheduledTest";
            }
            else if (status == 3)
            {
                endpoint = "/CloseScheduledTest";
            }

            request.Header.timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            request.Header.clientKey = clientKey;

            request.Body.LocationID = locationID;
            request.Body.PatientID = patientID;
            request.Body.OrderID = orderID;
            request.Body.ClientName = clientName;
            request.Body.Status = status;

            String strJson = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(strJson, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));


                    HttpResponseMessage resp = await client.PostAsync(url + endpoint, content);

                    return resp.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Get PMS URL
        /// </summary>
        /// <param name="sResultRequest"></param>
        /// <returns></returns>
        public async Task<string> GetPMSUrl(int clientID)
        {
            URLDataRequest request = new URLDataRequest() { Header = new HeaderModel(), Body = new URLDataRequestBody() };

            request.Header.timestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
            request.Header.clientKey = clientKey;

            request.Body.ClientID = clientID;

            String strJson = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(strJson, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

                    HttpResponseMessage resp = await client.PostAsync(url + "/GetPMSURL", content);
                    if (resp.IsSuccessStatusCode)
                    {
                        var resultString = resp.Content.ReadAsStringAsync().Result;
                        var results = JsonConvert.DeserializeObject<ResponseModel>(resultString);

                        return results.Body.Results.ToString();
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
