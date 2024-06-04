using log4net;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace VCheckViewerAPI.Lib.Util
{
    public class APILogging
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        public async void ApiLog(HttpContext context, object? responseBody)
        {
            object? requestBody = null;

            try
            {
                var requestHeader = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(context.Request.Headers));
                var responseHeader = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(context.Response.Headers));

                //context.Request.EnableBuffering();

                context.Request.Body.Seek(0, SeekOrigin.Begin);

                using (StreamReader stream = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    string body = await stream.ReadToEndAsync();
                    requestBody = JsonConvert.DeserializeObject(body);
                }

                context.Request.Body.Position = 0;


                using (var connection = new SqliteConnection(Host.CreateApplicationBuilder().Configuration.GetValue<string>("ApiLogPath")))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText = "Insert into apiLog(Request, Response) values('" + requestBody?.ToString() + "', '" + responseBody + "')";

                    //command.ExecuteReader();

                    //command.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error("Logging Error >>> ", ex);
            }

        }

        /// <summary>
        /// Log API Request & Response Payload
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public void ApiLog(object request, object response)
        {
            try
            {
                using (var connection = new SqliteConnection(Host.CreateApplicationBuilder().Configuration.GetValue<string>("ApiLogPath")))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText = "Insert into apiLog(Request, Response) values('" + request?.ToString() + "', '" + response.ToString() + "')";

                    command.ExecuteReader();

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error("Logging Error >>> ", ex);
            }

        }
    }
}
