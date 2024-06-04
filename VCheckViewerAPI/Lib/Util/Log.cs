using log4net;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using VCheckViewerAPI.Models;

namespace VCheckViewerAPI.Lib.Util
{
    public interface Log
    {
        void Debug(String msg);
        void Info(String msg);
        void Error(String msg, Exception? ex = null);
    }

    public class Logger : Log
    {
        private readonly ILog _logger;

        public Logger()
        {
            this._logger = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        }
        public void Debug(string msg)
        {
            this._logger?.Debug(msg);
        }
        public void Info(string msg)
        {
            this._logger?.Info(msg);
        }
        public void Error(string msg, Exception? ex = null)
        {
            this._logger?.Error(msg, ex);
        }

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
                Error("Logging Error >>> ", ex);
            }
            
        }

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
                Error("Logging Error >>> ", ex);
            }

        }
    }
}
