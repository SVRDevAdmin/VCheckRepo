using NHapi.Model.V23.Group;
using NHapi.Model.V231.Datatype;
using NHapi.Model.V24.Datatype;
using System.Reflection;
using VCheckListenerWorker;
using VCheckListenerWorker.Lib.DBContext;

/*
var builder = Host.CreateApplicationBuilder(args);

var sHostIP = builder.Configuration.GetSection("Listener:HostIP").Value;
var Ports = builder.Configuration.GetSection("Listener:Ports").GetChildren();

foreach (var p in Ports)
{
    var loggerFac = LoggerFactory.Create(x => x.AddConsole());
    ILogger<Worker> logger = loggerFac.CreateLogger<Worker>();

    builder.Services.AddHostedService<Worker>(x => new Worker(logger, sHostIP.ToString(), Convert.ToInt32(p.Value))));
    String abc = p.Value.ToString();
}
*/

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
