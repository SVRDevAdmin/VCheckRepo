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
    public class GeneralAPI
    {
        //string url = "http://vcheckcentral.inteleon.xyz/API/";
        string url = "https://localhost:7237/API/";

        /// <summary>
        /// Update Test Results to client server
        /// </summary>
        /// <param name="sResultRequest"></param>
        /// <returns></returns>
        public async Task<bool> SendData(string sRequestURL, Greywind.RequestMessage.UpdateResultRequest sResultRequest, string sUsername = "", string sPassword = "")
        {
            Boolean isSuccess = false;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

                    var byteArray = Encoding.ASCII.GetBytes(sUsername + ":" + sPassword);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    String strJson = JsonConvert.SerializeObject(sResultRequest);
                    HttpContent content = new StringContent(strJson, Encoding.UTF8, "application/json");

                    HttpResponseMessage resp = await client.PostAsync(sRequestURL, content);
                    if (resp.IsSuccessStatusCode)
                    //if (true)
                    {
                        String sResult = resp.Content.ReadAsStringAsync().Result;
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        ///// <summary>
        ///// Get PMS URL from central db
        ///// </summary>
        ///// <param name="sResultRequest"></param>
        ///// <returns></returns>
        //public async Task<string> GetPMSUrl(string clientKey, int clientID)
        //{
        //    RequestModel requestModel = new RequestModel() { Header = new HeaderModel() { timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"), clientKey = clientKey }, 
        //        Body = new RequestBody() { ClientID = clientID } };

        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

        //            HttpResponseMessage resp = client.PostAsJsonAsync(url + "GetAPIURL", requestModel).Result;
        //            if (resp.IsSuccessStatusCode)
        //            //if (true)
        //            {
        //                return resp.Content.ReadAsStringAsync().Result;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Get PMS URL from central db
        ///// </summary>
        ///// <param name="sResultRequest"></param>
        ///// <returns></returns>
        //public async Task<string> CreateLocation(string clientKey, LocationModel location)
        //{
        //    RequestModel requestModel = new RequestModel()
        //    {
        //        Header = new HeaderModel() { timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"), clientKey = clientKey },
        //        Body = new RequestBody() { LocationInfo = location }
        //    };

        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

        //            HttpResponseMessage resp = client.PostAsJsonAsync(url + "InsertLocation", requestModel).Result;
        //            if (resp.IsSuccessStatusCode)
        //            {
        //                return resp.Content.ReadAsStringAsync().Result;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Get PMS URL from central db
        ///// </summary>
        ///// <param name="sResultRequest"></param>
        ///// <returns></returns>
        //public async Task<string> GetScheduleListByLocationID(string clientKey, int locationID)
        //{
        //    RequestModel requestModel = new RequestModel()
        //    {
        //        Header = new HeaderModel() { timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"), clientKey = clientKey },
        //        Body = new RequestBody() { LocationID = locationID }
        //    };

        //    try
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

        //            HttpResponseMessage resp = client.PostAsJsonAsync(url + "GetScheduleListByLocation", requestModel).Result;
        //            if (resp.IsSuccessStatusCode)
        //            {
        //                return resp.Content.ReadAsStringAsync().Result;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}
