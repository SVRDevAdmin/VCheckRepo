using log4net.Config;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Reflection;
using VCheckViewerAPI.Lib.Util;
using VCheckViewerAPI.Models;
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

    Logger sLogger = new Logger();
    sLogger.Error("Internal Error >>> " ,error);

    var response = context.Response;
    response.ContentType = "application/json";
    response.Headers.Append("Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

    var clientKey = context.Request.Headers.Where(x => x.Key == "ClientKey").FirstOrDefault().Value.ToString();
    response.Headers.Append("ClientKey", clientKey != "" ? clientKey : "No Key");

    var model = new APIResponseModel("VV.9999", "Fail", "Internal Error", null);

    await context.Response.WriteAsJsonAsync(model);
}));


app.UseMiddleware<ApiHandlerMiddleware>();

app.MapControllers();

app.Run();
