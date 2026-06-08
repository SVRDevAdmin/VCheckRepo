using log4net.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCheck.Interface.API;
using VCheck.Lib.Data.Models;
using VCheckH6Listener.Lib.Logic;
using VCheckH6Listener.Lib.ValidationContext;

namespace VCheckH6Listener
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        Lib.Util.Logger sLogger;
        Socket sListener;
        public String sSystemName = "VCheckViewer Listener";
        public static string DeviceSerialNum { set; get; }

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                XmlConfigurator.Configure(log4net.LogManager.GetRepository(Assembly.GetEntryAssembly()),
                                          new FileInfo("log4Net.config"));
                sLogger = new Lib.Util.Logger();

                var configBuilder = Host.CreateApplicationBuilder();

                Socket sClient = sListener.Accept();

                string deviceType;
                IPEndPoint remoteIpEndPoint = sClient.RemoteEndPoint as IPEndPoint;

                string clientIpAddress = remoteIpEndPoint.Address.ToString();
                string clientIpPort = remoteIpEndPoint.Port.ToString();
                Console.WriteLine("Client connected from " + clientIpAddress + ":" + clientIpPort + ".");
                var device = TestResultRepository.GetDeviceByIPSerialNo(clientIpAddress, null, out deviceType);
                var clinicID = TestResultRepository.GetConfigurationByKey("ClinicID");

                var childSocket = new Thread(async () =>
                {
                    string tempData = "";
                    DeviceSerialNum = device != null && device.id != 0 ? device.DeviceName : null;

                    while (clinicID != null)
                    {
                        bool isConnected = true;
                        int byteLength = sClient.Available;
                        while (isConnected)
                        {
                            Thread.Sleep(1000);
                            var temp = sClient.Available;
                            if (temp == byteLength && temp != 0) { break; }
                            byteLength = temp;
                            isConnected = !(sClient.Poll(1000, SelectMode.SelectRead) && sClient.Available == 0);
                        }

                        if (!isConnected) { break; }

                        byte[] bBuffer = new byte[byteLength];

                        int s = sClient.Receive(bBuffer);
                        String sDataTemp = Encoding.ASCII.GetString(bBuffer, 0, s);
                        sDataTemp = sDataTemp.Replace("\u001c", "")
                                     .Replace("\n", "\r");
                        String sData = sDataTemp;

                        while (sClient.Available != 0)
                        {
                            s = sClient.Receive(bBuffer);
                            sDataTemp = Encoding.ASCII.GetString(bBuffer, 0, s);
                            sDataTemp = sDataTemp.Replace("\u001c", "")
                                         .Replace("\n", "\r");
                            if (string.IsNullOrEmpty(sDataTemp) || sDataTemp.Contains("MSH|")) { break; }
                            sData += sDataTemp;
                        }

                        if (!String.IsNullOrEmpty(sData) && sData != tempData)
                        {
                            tempData = sData;
                            Console.WriteLine("Data Message >> ");
                            Console.WriteLine(sData);

                            NHapi.Base.Parser.XMLParser sXMLParser = new NHapi.Base.Parser.DefaultXMLParser();
                            NHapi.Base.Parser.PipeParser sParser = new NHapi.Base.Parser.PipeParser() { ValidationContext = new CustomMessageValidation() };
                            NHapi.Base.Model.IMessage sIMessage = null;

                            try
                            {
                                sIMessage = sParser.Parse(sData.Trim());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Message failed during parsing: " + e.Message);
                            }

                            String sXMLMessage = String.Empty;
                            String sAckMessage = String.Empty;
                            String sFileName = String.Empty;

                            if (sIMessage != null)
                            {
                                sAckMessage = await SendAckMessage(sIMessage);
                                var sMessageByte = Encoding.UTF8.GetBytes(sAckMessage);
                                sClient.SendAsync(sMessageByte, SocketFlags.None);

                                Console.WriteLine("");
                                Console.WriteLine("Acknowledge Message Sent >> ");
                                Console.WriteLine(sAckMessage);

                                var processed = ProcessIMessage(sIMessage, sSystemName);

                                if (processed)
                                {
                                    sFileName = "TestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                                    sXMLMessage = sXMLParser.Encode(sIMessage);

                                    OutputMessage(configBuilder, sFileName, sData, sXMLMessage, sAckMessage);
                                }
                            }

                            Console.WriteLine("---------------------------------------------------------------------------------");
                        }
                    }

                    sClient.Close(); 
                    Console.WriteLine("Client disconnected from " + clientIpAddress + ":" + clientIpPort + ".");
                });

                childSocket.Start();

                if(deviceType == "H6")
                {
                    var childSocket2 = new Thread(async () =>
                    {
                        VCheckAPI vCheckAPI = new VCheckAPI();

                        while (clinicID != null)
                        {
                            var schedulesString = await vCheckAPI.GetScheduleListNotSent(clinicID.ConfigurationValue);
                            var schedulesExtended = string.IsNullOrEmpty(schedulesString) ? new List<ScheduledTestModelExtended>() : JsonConvert.DeserializeObject<List<ScheduledTestModelExtended>>(schedulesString);

                            if (schedulesExtended.Count > 0)
                            {
                                schedulesExtended = schedulesExtended.Where(x => x.IDAnalyzers.FirstOrDefault().Analyzers.Contains(deviceType)).ToList();
                                var order = await Lib.Logic.HL7.V231.OrderRepository.GenerateOrderMessageORM(schedulesExtended);

                                var testsMessageByte = Encoding.UTF8.GetBytes(order);
                                sClient.SendAsync(testsMessageByte, SocketFlags.None);

                                foreach (var schedule in schedulesExtended)
                                {
                                    await vCheckAPI.UpdateScheduleStatus(schedule.Schedule.LocationID, schedule.Schedule.PatientID, schedule.Schedule.ScheduleUniqueID.Split("-")[1], schedule.Schedule.CreatedBy, 1);
                                }

                            }

                            Thread.Sleep(5000);
                        }
                    });

                    childSocket2.Start();
                }
                
            }
            
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            String sHostIP = "";
            int iPortNo = 0;

            try
            {
                Console.WriteLine("Start Listener connection");

                sHostIP = GetAssignedIPAddress(out iPortNo);
                //sHostIP = GetIPAddressFromDatabase(out iPortNo);

                if (string.IsNullOrEmpty(sHostIP))
                {
                    Console.WriteLine("No IP Address assigned.");
                    Environment.Exit(1);
                    return base.StopAsync(cancellationToken);
                }
                else
                {
                    System.Net.IPEndPoint sIPEndPoint = System.Net.IPEndPoint.Parse(String.Concat(sHostIP, ":", iPortNo));

                    sListener = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                                                                                        System.Net.Sockets.SocketType.Stream,
                                                                                        System.Net.Sockets.ProtocolType.Tcp);
                    sListener.Bind(sIPEndPoint);
                    sListener.Listen(3);

                    Console.WriteLine("Listener Start Successful.");
                    Console.WriteLine("IP Address : " + sHostIP);
                    Console.WriteLine("Port No : " + iPortNo);
                    Console.WriteLine("-------------------------------------------------------------------");

                    return base.StartAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("StartAsync >>> " + ex.ToString());
                sLogger.Error("StopAsync >>>> " + ex.ToString());
                Environment.Exit(1);
                return base.StopAsync(cancellationToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Initiated Stop Listener connection.");

                sListener.Disconnect(true);
                sListener.Dispose();

                //_logger.LogInformation("Listener connection closed.");
                Console.WriteLine("Listener connection closed.");

            }
            catch (Exception ex)
            {
                _logger.LogError("StopAsync >>>> " + ex.ToString());
                sLogger.Error("StopAsync >>>> " + ex.ToString());
            }

            return base.StopAsync(cancellationToken);
        }



        /// <summary>
        /// Process HL7 Message
        /// </summary>
        /// <param name="sIMessage"></param>
        /// <param name="sSystemName"></param>
        /// <returns></returns>
        private Boolean ProcessIMessage(NHapi.Base.Model.IMessage sIMessage, String sSystemName)
        {
            Boolean isCompleted = false;

            switch (sIMessage.Version)
            {
                case "2.3.1":
                    if (sIMessage.Message.Message.GetType() == typeof(NHapi.Model.V231.Message.ORU_R01))
                    {
                        Lib.Logic.HL7.V231.HL7Repository.ProcessMessageAsync(sIMessage, sSystemName);
                        isCompleted = true;
                    }
                    break;

                default:
                    break;
            }

            return isCompleted;
        }

        /// <summary>
        /// Output data to file
        /// </summary>
        /// <param name="sBuilder"></param>
        /// <param name="sFileName"></param>
        /// <param name="sData"></param>
        /// <param name="sXMLMessage"></param>
        /// <param name="sAckMessage"></param>
        public void OutputMessage(HostApplicationBuilder sBuilder, String sFileName, String sData, String sXMLMessage, String sAckMessage)
        {
            try
            {
                String sOutputPathHL7 = sBuilder.Configuration.GetSection("FileOutput:HL7").Value;
                String sOutputPathXML = sBuilder.Configuration.GetSection("FileOutput:XML").Value;
                String sOutputPathACK = sBuilder.Configuration.GetSection("FileOutput:ACK").Value;

                if (!String.IsNullOrEmpty(sOutputPathHL7))
                {
                    String outputPathHL7 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathHL7);
                    if (!Directory.Exists(outputPathHL7))
                    {
                        Directory.CreateDirectory(outputPathHL7);
                    }
                    File.WriteAllText(outputPathHL7 + sFileName + ".hl7", sData, System.Text.Encoding.ASCII);
                }

                // Output to XML file
                if (!String.IsNullOrEmpty(sOutputPathXML))
                {
                    String outputPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathXML);
                    if (!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                    File.WriteAllText(outputPath + sFileName + ".xml", sXMLMessage, System.Text.Encoding.ASCII);
                }

                if (!String.IsNullOrEmpty(sOutputPathACK))
                {
                    String outputPathACK = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathACK);
                    if (!Directory.Exists(outputPathACK))
                    {
                        Directory.CreateDirectory(outputPathACK);
                    }
                    File.WriteAllText(outputPathACK + sFileName + ".hl7", sAckMessage, System.Text.Encoding.ASCII);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Function OutputMessage >>> " + ex.ToString());
                sLogger.Error("StopAsync >>>> " + ex.ToString());
            }

        }

        private async Task<String> SendAckMessage(NHapi.Base.Model.IMessage sIMessage)
        {
            String sMessage = String.Empty;

            switch (sIMessage.Version)
            {
                case "2.3.1":
                    if (sIMessage.Message.Message.GetType() == typeof(NHapi.Model.V231.Message.ORU_R01))
                    {
                        sMessage = Lib.Logic.HL7.V231.AckRepository.GenerateAcknowlegeMessageORU(sIMessage);
                    }
                    else if (sIMessage.Message.Message.GetType() == typeof(NHapi.Model.V231.Message.QRY_Q02))
                    {
                        Lib.Logic.HL7.V231.AckRepository ackRepository = new Lib.Logic.HL7.V231.AckRepository();
                        sMessage = await ackRepository.GenerateAcknowlegeMessageQRY(sIMessage);
                    }
                    break;

                default:
                    break;
            }

            return sMessage;
        }

        public static string GetAssignedIPAddress(out int port)
        {
            port = 6666;

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString() != "127.0.0.1")
                {
                    return ip.ToString();
                }
            }

            return "";
        }
    }
}
