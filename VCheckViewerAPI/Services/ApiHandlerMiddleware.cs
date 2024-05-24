using VCheck.Lib.Data;
using VCheck.Lib.Data.Models;
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
            var response = context.Response;
            response.ContentType = "application/json";
            response.Headers.Append("Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            var clientKey = context.Request.Headers.Where(x => x.Key == "ClientKey").FirstOrDefault().Value.ToString();
            response.Headers.Append("ClientKey", clientKey != "" ? clientKey : "No Key");

            if (_apiRepository.Authenticate(clientKey))
            {
                await _next(context);
            }
            else
            {
                await response.WriteAsJsonAsync(new APIResponseModel("VV.0003", "Fail", "Unauthorized Request", null));
            }               
        }
    }
}
