using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using System.Text.Json.Serialization;
using VCheck.Lib.Data;
using VCheck.Lib.Data.Models;
using VCheckViewerAPI.Lib.Util;
using VCheckViewerAPI.Models;

namespace VCheckViewerAPI.Services
{
    public class ApiHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private ApiRepository _apiRepository = new ApiRepository();

        public ApiHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //var response = context.Response;
            //object? responseBodyJson;
            //response.ContentType = "application/json";
            //response.Headers.Append("Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            //var clientKey = context.Request.Headers.Where(x => x.Key == "ClientKey").FirstOrDefault().Value.ToString();
            //var timestamp = context.Request.Headers.Where(x => x.Key == "Timestamp").FirstOrDefault().Value.ToString();
            //response.Headers.Append("ClientKey", clientKey != "" ? clientKey : "No Key");

            context.Request.EnableBuffering();

            //if (_apiRepository.Authenticate(clientKey) && timestamp != "")
            //{
            //using (var swapStream = new MemoryStream())
            //    {
                    //var originalResponseBody = response.Body;
                    //response.Body = swapStream;

                    await _next(context);

                    //swapStream.Seek(0, SeekOrigin.Begin);
                    //string responseBody = new StreamReader(swapStream).ReadToEnd();
                    //responseBodyJson = JsonConvert.DeserializeObject(responseBody);

                    //swapStream.Seek(0, SeekOrigin.Begin);
                    //await swapStream.CopyToAsync(originalResponseBody);
                    //response.Body = originalResponseBody;
                //}
            //}
            //else
            //{
            //    var responseData = new APIResponseModel("VV.0003", "Fail", "Unauthorized Request", null);
            //    responseBodyJson = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(responseData));
            //    await response.WriteAsJsonAsync(responseData);
            //}

            //Logger sLogger = new Logger();
            //sLogger.ApiLog(context, responseBodyJson);
        }
    }
}
