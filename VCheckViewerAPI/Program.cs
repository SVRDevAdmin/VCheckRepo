using log4net.Config;
using Microsoft.AspNetCore.Diagnostics;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;
using System.Reflection;
using System.Text;
using VCheckViewerAPI.Lib.Util;
using VCheckViewerAPI.Message.General;
using VCheckViewerAPI.Services;


XmlConfigurator.Configure(log4net.LogManager.GetRepository(Assembly.GetEntryAssembly()),
                                  new FileInfo("log4Net.config"));

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.UseExceptionHandler(a => a.Run(async context =>
{
    var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    string? ClientKey;
    object? requestJson = null;

    Logger sLogger = new Logger();
    sLogger.Error("Internal Error >>> " ,error);

    //context.Request.EnableBuffering();

    context.Request.Body.Seek(0, SeekOrigin.Begin);

    using (StreamReader stream = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
    {
        string body = await stream.ReadToEndAsync();
        requestJson = JsonConvert.DeserializeObject(body);
        ClientKey = JsonConvert.DeserializeObject<HeaderModel>(body)?.clientKey;
    }

    context.Request.Body.Position = 0;

    var responseHeader = new HeaderModel() { timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), clientKey = ClientKey };
    var responseBody = new ResponseBody() { ResponseCode = "VV.9999", ResponseStatus = "Fail", ResponseMessage = "Internal Error", Results = null };
    var response = new ResponseModel() { Header = responseHeader, Body = responseBody };

    var responseJson = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(response));

    //sLogger.ApiLog(requestJson, responseJson);
    APILogging sAPILogging = new APILogging();
    sAPILogging.ApiLog(context, responseJson);
}));


app.Use(async (context, next) =>
    {
        context.Request.EnableBuffering();

        await next();
    });

//app.UseMiddleware<ApiHandlerMiddleware>();

app.MapControllers();

app.Run();
