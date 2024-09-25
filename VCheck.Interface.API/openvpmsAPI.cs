using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace VCheck.Interface.API
{
    public class openvpmsAPI
    {
        public static IConfiguration iConfig;
        public static String? sUsername;
        public static String? sPassword;

        public openvpmsAPI()
        {
            var sBuilder = new ConfigurationBuilder();
            sBuilder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            iConfig = sBuilder.Build();

            sUsername = iConfig.GetSection("OpenVPMSCredentials:username").Value;
            sPassword = iConfig.GetSection("OpenVPMSCredentials:password").Value;
        }

        public openvpms.ResponseMessage.RetrieveBookingResponse? RetrieveBooking(String sID)
        {
            String? sRequestURL = iConfig.GetSection("OpenVPMS:RetrieveBookingURL").Value;
            sRequestURL = sRequestURL.Replace("{id}", sID);

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
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<openvpms.ResponseMessage.RetrieveBookingResponse>(strResult);
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

        public Boolean SubmitBooking(openvpms.RequestMessage.SubmitBookingRequest sInput)
        {
            String? sRequestURL = iConfig.GetSection("OpenVPMS:SubmitBookingURL").Value;
            Boolean isSuccess = false;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

                    var byteArray = Encoding.ASCII.GetBytes(sUsername + ":" + sPassword);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    HttpResponseMessage resp = client.PostAsJsonAsync(sRequestURL, sInput).Result;
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
}
