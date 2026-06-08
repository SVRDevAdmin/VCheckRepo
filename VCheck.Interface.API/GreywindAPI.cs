using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using VCheck.Interface.API.General;

namespace VCheck.Interface.API
{
    public class GreywindAPI
    {
        public static IConfiguration iConfig;
        public static String? sUsername = "svrtech";
        public static String? sPassword = "V7FZ+fu9sQ9*";

        public GreywindAPI()
        {
            //var sBuilder = new ConfigurationBuilder();
            //sBuilder.SetBasePath(Directory.GetCurrentDirectory())
            //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //iConfig = sBuilder.Build();

            //sUsername = iConfig.GetSection("GreywindPMSCredentials:username").Value;
            //sPassword = iConfig.GetSection("GreywindPMSCredentials:password").Value;
        }

        /// <summary>
        /// Pull Order Info by Order ID
        /// </summary>
        /// <param name="sOrderID"></param>
        /// <returns></returns>
        public Greywind.ResponseMessage.FetchOrderResponse? FetchOrder(String sOrderID, string sURL)
        {
            //String? sRequestURL = string.IsNullOrEmpty(sURL) ? iConfig.GetSection("GreywindPMS:FetchOrderURL").Value : sURL;
            //sRequestURL = sRequestURL.EndsWith("/") ? sRequestURL : sRequestURL + "/";
            //sRequestURL = string.IsNullOrEmpty(sURL) ? sRequestURL.Replace("{orderid}", sOrderID) : sRequestURL + "order/fetch/" + sOrderID;

            String? sRequestURL = sURL + "/order/fetch/" + sOrderID;


            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json;charset=utf-8"));

                    var byteArray = Encoding.ASCII.GetBytes(sUsername + ":" + sPassword);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    HttpResponseMessage resp = client.GetAsync(sRequestURL).Result;
                    if (resp.IsSuccessStatusCode)
                    {
                        String strResult = resp.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<Greywind.ResponseMessage.FetchOrderResponse>(strResult);
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
        /// Update Test Results to Greywind
        /// </summary>
        /// <param name="sResultRequest"></param>
        /// <returns></returns>
        public Boolean UpdateResult(Greywind.RequestMessage.UpdateResultRequest sResultRequest, String sOrderID, string sURL = "")
        {
            //String? sRequestURL = string.IsNullOrEmpty(sURL) ? iConfig.GetSection("GreywindPMS:UpdateResultURL").Value : sURL;
            //sRequestURL = sRequestURL.EndsWith("/") ? sRequestURL : sRequestURL + "/";
            //sRequestURL = string.IsNullOrEmpty(sURL) ? sRequestURL.Replace("{orderid}", sOrderID) : sRequestURL + "order/results/" + sOrderID;

            //sUsername = string.IsNullOrEmpty(sUsernameInput) ? sUsername : sUsernameInput;
            //sPassword = string.IsNullOrEmpty(sPasswordInput) ? sPassword : sPasswordInput;

            String? sRequestURL = sURL + "/order/results/" + sOrderID;

            //sUsername = sUsernameInput;
            //sPassword = sPasswordInput;

            Boolean isSuccess = false;

            String strJson = JsonConvert.SerializeObject(sResultRequest);

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

                    var byteArray = Encoding.ASCII.GetBytes(sUsername + ":" + sPassword);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Allow all Unicode
                    };

                    //HttpResponseMessage resp = client.PostAsJsonAsync(sRequestURL, sResultRequest).Result;
                    HttpResponseMessage resp = client.PostAsJsonAsync(sRequestURL, sResultRequest, options).Result;
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

        /// <summary>
        /// Cancel Test Results to Greywind
        /// </summary>
        /// <param name="sResultRequest"></param>
        /// <returns></returns>
        public Boolean UpdateScheduleStatus(String sOrderID, String sAccessionNo, string sStatus, string sURL = "")
        {
            //String? sRequestURL = string.IsNullOrEmpty(sURL) ? iConfig.GetSection("GreywindPMS:UpdateScheduleStatusURL").Value : sURL;
            //sRequestURL = sRequestURL.EndsWith("/") ? sRequestURL : sRequestURL + "/";
            //sRequestURL = string.IsNullOrEmpty(sURL) ? sRequestURL.Replace("{orderid}", sOrderID) : sRequestURL + "order/status-updates/" + sOrderID;

            //sUsername = string.IsNullOrEmpty(sUsernameInput) ? sUsername : sUsernameInput;
            //sPassword = string.IsNullOrEmpty(sPasswordInput) ? sPassword : sPasswordInput;

            String? sRequestURL = sURL + "/order/status-updates/";

            //sUsername = sUsernameInput;
            //sPassword = sPasswordInput;

            Boolean isSuccess = false;

            var sResultRequest = new List<StatusObject>() { new StatusObject() { ordernumber = sOrderID, accessionnumber = sAccessionNo, status = sStatus } };

            String strJson = JsonConvert.SerializeObject(sResultRequest);

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

                    var byteArray = Encoding.ASCII.GetBytes(sUsername + ":" + sPassword);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    HttpResponseMessage resp = client.PostAsJsonAsync(sRequestURL, sResultRequest).Result;
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

        /// <summary>
        /// Get Clinic Info
        /// </summary>
        /// <param name="sResultRequest"></param>
        /// <returns></returns>
        public async Task<Boolean> ClinicExist(String sClinicID, string sURL = "")
        {
            return !string.IsNullOrEmpty(await GetClinicInfo(sClinicID, sURL));
        }

        public async Task<String> GetClinicInfo(String sClinicID, string sURL = "")
        {
            String? sRequestURL = sURL + "/sync/clinic/" + sClinicID;

            var ClinicInfoString = "";

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

                    var byteArray = Encoding.ASCII.GetBytes(sUsername + ":" + sPassword);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    HttpResponseMessage resp = await client.GetAsync(sRequestURL);
                    if (resp.IsSuccessStatusCode)
                    {
                        ClinicInfoString = resp.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                ClinicInfoString = null;
            }

            return ClinicInfoString;
        }

        /// <summary>
        /// Get Clinic Info
        /// </summary>
        /// <param name="sResultRequest"></param>
        /// <returns></returns>
        public async Task<Boolean> InsertClinicInfo(Greywind.RequestMessage.InsertLocationRequest sResultRequest, string sURL, String sClinicID = "")
        {
            String? sRequestURL = sURL + "/sync/clinic/" + sClinicID;

            Boolean isSuccess = false;

            String strJson = JsonConvert.SerializeObject(sResultRequest);
            HttpContent content = new StringContent(strJson, Encoding.UTF8, "application/json");

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

                    var byteArray = Encoding.ASCII.GetBytes(sUsername + ":" + sPassword);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    HttpResponseMessage resp = await client.PostAsync(sRequestURL, content);
                    if (resp.IsSuccessStatusCode)
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
    }

    public class StatusObject
    {
        public string ordernumber { get; set; }
        public string accessionnumber { get; set; }
        public string status { get; set; }
    }
}
