using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VCheck.Interface.API.General;
using Newtonsoft.Json;

namespace VCheck.Interface.API
{
    public class GreywindAPI
    {
        public static IConfiguration iConfig;
        public static String? sUsername;
        public static String? sPassword;

        public GreywindAPI()
        {
            var sBuilder = new ConfigurationBuilder();
            sBuilder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            iConfig = sBuilder.Build();

            sUsername = iConfig.GetSection("GreywindPMSCredentials:username").Value;
            sPassword = iConfig.GetSection("GreywindPMSCredentials:password").Value;
        }

        /// <summary>
        /// Pull Order Info by Order ID
        /// </summary>
        /// <param name="sOrderID"></param>
        /// <returns></returns>
        public Greywind.ResponseMessage.FetchOrderResponse? FetchOrder(String sOrderID)
        {
            String? sRequestURL = iConfig.GetSection("GreywindPMS:FetchOrderURL").Value;
            sRequestURL = sRequestURL.Replace("{orderid}", sOrderID);

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
        public Boolean UpdateResult(Greywind.RequestMessage.UpdateResultRequest sResultRequest, String sOrderID)
        {
            String? sRequestURL = iConfig.GetSection("GreywindPMS:UpdateResultURL").Value;
            sRequestURL = sRequestURL.Replace("{orderid}", sOrderID);

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

                    HttpResponseMessage resp = client.PostAsJsonAsync(sRequestURL, sResultRequest).Result;
                    if (resp.IsSuccessStatusCode)
                    //if (true)
                    {
                        //String sResult = resp.Content.ReadAsStringAsync().Result;
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
}
