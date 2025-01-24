using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using VCheck.Interface.API.General;
using Microsoft.AspNetCore.WebUtilities;

namespace VCheck.Interface.API
{
    public class ezyVetAPI
    {
        public static IConfiguration iConfig;

        public ezyVetAPI()
        {
            var sBuilder = new ConfigurationBuilder();
            sBuilder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            iConfig = sBuilder.Build();
        }

        #region Token
        /// <summary>
        /// Request Access Token
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public ezyVet.ResponseMessage.AccessTokenObject? GetAccessToken(ezyVet.RequestMessage.AccessTokenRequest sInput)
        {
            String? sRequestURL = iConfig.GetSection("ezyVet:GetAccessTokenURL").Value;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage resp = client.PostAsJsonAsync(sRequestURL, sInput).Result;
                    if (resp.IsSuccessStatusCode)
                    {
                        String strResult = resp.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<ezyVet.ResponseMessage.AccessTokenObject>(strResult);
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
        #endregion


        #region Appointment
        /// <summary>
        /// Get Appointment List
        /// </summary>
        /// <param name="sInput"></param>
        /// <returns></returns>
        public ezyVet.ResponseMessage.AppointmentObject? GetAppointmentList(ezyVet.RequestMessage.GetAppointmentRequest sInput, String sToken)
        {
            String? sRequestURL = iConfig.GetSection("ezyVet:GetAppointmentListURL").Value;

            try
            {
                Dictionary<String, String> sInputQuery = UtilityLib.ToDictionary<String>(sInput);

                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri(sRequestURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("authorization", "Bearer " + sToken);

                    HttpResponseMessage resp = client.GetAsync(QueryHelpers.AddQueryString(sRequestURL, sInputQuery)).Result;
                    if (resp.IsSuccessStatusCode)
                    {
                        String strResult = resp.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<ezyVet.ResponseMessage.AppointmentObject>(strResult);
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
        /// Get Appointment Info By ID
        /// </summary>
        /// <param name="AppointmentID"></param>
        /// <returns></returns>
        public ezyVet.ResponseMessage.AppointmentObject? GetAppointmentByID(String AppointmentID, String sToken)
        {
            String? sRequestURL = iConfig.GetSection("ezyVet:GetAppointmentByIDURL").Value;
            sRequestURL = sRequestURL.Replace("{id}", AppointmentID);

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(sRequestURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("authorization", "Bearer " + sToken);

                    HttpResponseMessage resp = client.GetAsync(sRequestURL).Result;
                    if (resp.IsSuccessStatusCode)
                    {
                       String strResult = resp.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<ezyVet.ResponseMessage.AppointmentObject>(strResult);
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
        /// Update Appointment Status
        /// </summary>
        /// <param name="sInput"></param>
        /// <param name="sToken"></param>
        /// <param name="AppointmentID"></param>
        /// <returns></returns>
        public ezyVet.ResponseMessage.AppointmentUpdateStatusObject? UpdateAppointmentStatus(ezyVet.RequestMessage.UpdateAppointmentRequest sInput, String sToken, String AppointmentID)
        {
            String? sRequestURL = iConfig.GetSection("ezyVet:UpdateAppointmentStatusURL").Value;
            sRequestURL = sRequestURL.Replace("{id}", AppointmentID);

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("authorization", "Bearer " + sToken);

                    HttpResponseMessage resp = client.PatchAsJsonAsync(sRequestURL, sInput).Result;
                    if (resp.IsSuccessStatusCode)
                    {
                        String strResult = resp.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<ezyVet.ResponseMessage.AppointmentUpdateStatusObject>(strResult);
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
        /// Get Appointment Type List
        /// </summary>
        /// <param name="sToken"></param>
        /// <returns></returns>
        public ezyVet.ResponseMessage.AppointmentTypeObject? GetAppointmentTypeList(String sToken)
        {
            String? sRequestURL = iConfig.GetSection("ezyVet:GetAppointmentTypeURL").Value;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("authorization", "Bearer " + sToken);

                    HttpResponseMessage resp = client.GetAsync(sRequestURL).Result;
                    if (resp.IsSuccessStatusCode)
                    {
                        String strResult = resp.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<ezyVet.ResponseMessage.AppointmentTypeObject>(strResult);
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
        /// Get Appointment Status List
        /// </summary>
        /// <param name="sToken"></param>
        /// <returns></returns>
        public ezyVet.ResponseMessage.AppointmentStatusObject? GetAppointmentStatusList(String sToken)
        {
            String? sRequestURL = iConfig.GetSection("ezyVet:GetAppointmentStatusURL").Value;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("authorization", "Bearer " + sToken);

                    HttpResponseMessage resp = client.GetAsync(sRequestURL).Result;
                    if (resp.IsSuccessStatusCode)
                    {
                        String strResult = resp.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<ezyVet.ResponseMessage.AppointmentStatusObject>(strResult);
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
        #endregion


        #region Diagnotic Result
        /// <summary>
        /// Create Diagnostic Result
        /// </summary>
        /// <param name="sInput"></param>
        /// <param name="sToken"></param>
        /// <returns></returns>
        public ezyVet.ResponseMessage.DianogsticResultObject? CreateDiagnosticResult(ezyVet.RequestMessage.CreateDiagnosticRequest sInput, String sToken)
        {
            String? sRequestURL = iConfig.GetSection("ezyVet:CreateDiagnosticResultURL").Value;

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("authorization", "Bearer " + sToken);

                    HttpResponseMessage resp = client.PostAsJsonAsync(sRequestURL, sInput).Result;
                    if (resp.IsSuccessStatusCode)
                    {
                        String strResult = resp.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<ezyVet.ResponseMessage.DianogsticResultObject>(strResult);
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
        /// Get Diagnostic Result
        /// </summary>
        /// <param name="sInput"></param>
        /// <param name="sToken"></param>
        /// <returns></returns>
        public ezyVet.ResponseMessage.DiagnosticListingObject? GetDiagnosticResult(ezyVet.RequestMessage.GetDiagnosticResultRequest sInput, String sToken)
        {
            String? sRequestURL = iConfig.GetSection("ezyVet:GetDiagnosticResultURL").Value;

            try
            {
                Dictionary<String, String> sInputQuery = UtilityLib.ToDictionary<String>(sInput);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("authorization", "Bearer " + sToken);

                    HttpResponseMessage resp = client.GetAsync(QueryHelpers.AddQueryString(sRequestURL, sInputQuery)).Result;
                    if (resp.IsSuccessStatusCode)
                    {
                        String strResult = resp.Content.ReadAsStringAsync().Result;
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<ezyVet.ResponseMessage.DiagnosticListingObject>(strResult);
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
        #endregion
    }
}
