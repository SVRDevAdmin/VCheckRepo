using log4net.Config;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Mysqlx.Crud;
using Newtonsoft.Json;
using NHapi.Base.Parser;
using NHapi.Base.Util;
using NHapi.Base.Validation.Implementation;
using NHapi.Model.V23.Segment;
using NHapiTools.Model.V26.Segment;
using Org.BouncyCastle.Asn1;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using VCheck.Interface.API;
using VCheck.Lib.Data.Models;
using VCheckListenerWorker.Lib.Logic;
using VCheckListenerWorker.Lib.Logic.HL7.V231;
using VCheckListenerWorker.Lib.Models;
using VCheckListenerWorker.Lib.ValidationContext;

namespace VCheckListenerWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        System.Net.Sockets.Socket sListener;
        VCheckListenerWorker.Lib.Util.Logger sLogger;

        public String sSystemName = "VCheckViewer Listener";

        public static MainModel MainModel { get; } = new MainModel();

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Main Logic process the data
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
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
                    MainModel.DeviceSerialNum = device != null && device.id != 0 ? device.DeviceSerialNo : null;
                    MainModel.DeviceType = deviceType;
                    bool continueLoop = true;

                    //while (clinicID != null && continueLoop)
                    while (continueLoop)
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
                        String sDataTemp = Encoding.UTF8.GetString(bBuffer, 0, s);
                        sDataTemp = sDataTemp.Replace("\u001c", "")
                                     .Replace("\n", "\r");
                        String sData = sDataTemp;

                        while (sClient.Available != 0)
                        {
                            s = sClient.Receive(bBuffer);
                            sDataTemp = Encoding.UTF8.GetString(bBuffer, 0, s);
                            sDataTemp = sDataTemp.Replace("\u001c", "")
                                         .Replace("\n", "\r");
                            //if (string.IsNullOrEmpty(sDataTemp) || sDataTemp.Contains("MSH|")) { break; }
                            if (string.IsNullOrEmpty(sDataTemp)) { break; }
                            sData += sDataTemp;
                            Thread.Sleep(500);
                        }

                        if (!String.IsNullOrEmpty(sData) && sData != tempData)
                        {
                            tempData = sData;

                            NHapi.Base.Parser.XMLParser sXMLParser = new NHapi.Base.Parser.DefaultXMLParser();
                            NHapi.Base.Parser.PipeParser sParser = new NHapi.Base.Parser.PipeParser() { ValidationContext = new CustomMessageValidation() };
                            NHapi.Base.Model.IMessage sIMessage = null;
                            //List<NHapi.Base.Model.IMessage> sIMessageList = new List<NHapi.Base.Model.IMessage>();
                            bool continueProcessData = false;

                            try
                            {
                                if((sData.Contains("ORU^R01") && sData.Contains("OBX")) || !sData.Contains("ORU^R01"))
                                {
                                    continueProcessData = true;
                                }

                                //NHapi.Base.Model.IMessage sIMessage = null;
                                //string[] rawMessages = sData.Split(new[] { '\v' }, StringSplitOptions.RemoveEmptyEntries);

                                //if (sData.Contains("Hypoadrenocorticism")) { sData = sData.Replace("Hypoadrenocorticism", "").Replace("Consistent with ", "Consistent with Hypoadrenocorticism"); }

                                var lines = sData.Split("\r", StringSplitOptions.RemoveEmptyEntries);

                                for (int i = 0; i < lines.Length; i++)
                                {
                                    if (!lines[i].Contains("|")) { lines[i - 1] = lines[i - 1] + lines[i]; lines[i] = ""; }
                                }

                                sData = string.Join("\r", lines);

                                sIMessage = sParser.Parse(sData.Trim());

                                //if(rawMessages.Count() == 1) { sIMessage = sParser.Parse(sData.Trim()); }
                                //else
                                //{
                                //    foreach (string message in rawMessages)
                                //    {
                                //        sIMessage = sParser.Parse(message.Trim());
                                //        sIMessageList.Add(sIMessage);
                                //    }
                                //}

                                //foreach (string message in rawMessages)
                                //{
                                //    sIMessage = sParser.Parse(message.Trim());
                                //    sIMessageList.Add(sIMessage);
                                //}

                            }
                            catch (Exception e)
                            {
                                _logger.LogError("Message failed during parsing: " + e.Message);
                                Console.WriteLine("Message failed during parsing: " + e.Message);
                            }

                            String sXMLMessage = String.Empty;
                            String sAckMessage = String.Empty;
                            String sFileName = "TestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                            if (sIMessage != null)
                            {
                                sAckMessage = await SendAckMessage(sIMessage);
                                var sMessageByte = Encoding.UTF8.GetBytes(sAckMessage);
                                sClient.Send(sMessageByte, SocketFlags.None);

                                if (deviceType != "H6" && deviceType != "U3")
                                {
                                    sClient.Close();
                                    Console.WriteLine("Client disconnected from " + clientIpAddress + ":" + clientIpPort + ".");
                                    continueLoop = false;
                                }

                                if (continueProcessData)
                                {
                                    //var processed = ProcessIMessage(sIMessage, sSystemName);
                                    //var processed = ProcessIMessage(sIMessageList, sSystemName);
                                    Task.Run(() => ProcessIMessage(sIMessage, sSystemName));
                                    sXMLMessage = sXMLParser.Encode(sIMessage);

                                    //foreach (var message in sIMessageList)
                                    //{
                                    //    sXMLMessage = string.IsNullOrEmpty(sXMLMessage) ? sXMLMessage + sXMLParser.Encode(message) : sXMLMessage + sXMLParser.Encode(message).Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                                    //}

                                    //if(sIMessageList.Count() > 1) { sXMLMessage = sXMLMessage.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<MultiResult>\n") + "\n</MultiResult>"; }
                                    OutputMessage(configBuilder, sFileName, sData, sXMLMessage, sAckMessage);
                                }
                            }
                            else
                            {
                                sFileName = "ErrorTestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                                OutputMessage(configBuilder, sFileName, sData, null, null);
                            }

                            Console.WriteLine("---------------------------------------------------------------------------------");
                        }
                    }

                    if (deviceType == "H6" || deviceType == "U3")
                    {
                        sClient.Close();
                        Console.WriteLine("Client disconnected from " + clientIpAddress + ":" + clientIpPort + ".");
                    }

                    //sClient.Close();
                    //Console.WriteLine("Client disconnected from " + clientIpAddress + ":" + clientIpPort + ".");
                });

                childSocket.Start();

                if (deviceType == "H6")
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
                                schedulesExtended = schedulesExtended.Where(x => x.IDAnalyzers.Where(y => y.Analyzers == "H6").Count() != 0).ToList();
                                var order = await OrderRepository.GenerateOrderMessageORM(schedulesExtended);

                                if (!string.IsNullOrEmpty(order))
                                {
                                    var testsMessageByte = Encoding.UTF8.GetBytes(order);
                                    sClient.SendAsync(testsMessageByte, SocketFlags.None);

                                    foreach (var schedule in schedulesExtended)
                                    {
                                        await vCheckAPI.UpdateScheduleStatus(schedule.Schedule.LocationID, schedule.Schedule.PatientID, schedule.Schedule.ScheduleUniqueID.Split("-")[1], schedule.Schedule.CreatedBy, 1);
                                        await vCheckAPI.UpdateScheduleAnalyzer("H6", schedule.Schedule.ScheduleUniqueID);
                                    }
                                }

                            }


                            Thread.Sleep(TimeSpan.FromMinutes(5));
                            //Thread.Sleep(5000);
                        }
                    });

                    childSocket2.Start();
                }

            }

        }

        /// <summary>
        /// When program start execution
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            String sHostIP = "";
            int iPortNo = 0;

            try
            {
                Console.WriteLine("Start Listener connection");

                //var builder = Host.CreateApplicationBuilder();
                //sHostIP = builder.Configuration.GetSection("Listener:HostIP").Value;                
                //iPortNo = Convert.ToInt32(builder.Configuration.GetSection("Listener:Port").Value);

                //sHostIP = "169.254.98.184";
                //iPortNo = 8585;

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
                Environment.Exit(1);
                return base.StopAsync(cancellationToken);
            }           
        }

        /// <summary>
        /// When program stopped
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
                case "2.6":
                    Lib.Logic.HL7.V26.HL7Repository.ProcessMessageAsync(sIMessage, sSystemName);
                    break;

                case "2.5.1":
                    Lib.Logic.HL7.V251.HL7Repository.ProcessMessage(sIMessage, sSystemName);
                    break;

                case "2.3.1":
                    if (sIMessage.Message.Message.GetType() == typeof(NHapi.Model.V231.Message.ORU_R01))
                    {
                        Lib.Logic.HL7.V231.HL7Repository.ProcessMessageAsync(sIMessage, sSystemName);
                    }
                    break;

                default:
                    break;
            }

            return isCompleted;
        }

        /// <summary>
        /// Generate Ack Message
        /// </summary>
        /// <param name="sIMessage"></param>
        /// <returns></returns>
        private async Task<String> SendAckMessage(NHapi.Base.Model.IMessage sIMessage)
        {
            String sMessage = String.Empty;

            switch (sIMessage.Version)
            {
                case "2.6":
                    sMessage = Lib.Logic.HL7.V26.AckRepository.GenerateAcknowlegeMessage(sIMessage);

                    break;

                case "2.5.1":
                    if (sIMessage.Message.Message.GetType() == typeof(NHapi.Model.V251.Message.QBP_Q11))
                    {
                        sMessage = Lib.Logic.HL7.V251.AckRepository.GenerateAcknowlegeMessageQBP(sIMessage);
                    }
                    else if(sIMessage.Message.Message.GetType() == typeof(NHapi.Model.V251.Message.OUL_R22))
                    {
                        sMessage = Lib.Logic.HL7.V251.AckRepository.GenerateAcknowlegeMessage(sIMessage);
                    }
                    
                    break;

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

                if (!String.IsNullOrEmpty(sOutputPathHL7) && sData != null)
                {
                    String outputPathHL7 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathHL7);
                    if (!Directory.Exists(outputPathHL7))
                    {
                        Directory.CreateDirectory(outputPathHL7);
                    }
                    File.WriteAllText(outputPathHL7 + sFileName + ".hl7", sData, System.Text.Encoding.ASCII);
                }

                // Output to XML file
                if (!String.IsNullOrEmpty(sOutputPathXML) && sXMLMessage != null)
                {
                    String outputPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathXML);
                    if (!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                    File.WriteAllText(outputPath + sFileName + ".xml", sXMLMessage, System.Text.Encoding.ASCII);
                }

                if (!String.IsNullOrEmpty(sOutputPathACK) && sAckMessage != null)
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
            }

        }

        /// <summary>
        /// Get current IP Address
        /// </summary>
        public static string GetAssignedIPAddress(out int port)
        {
            port = 8585;

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString() != "127.0.0.1")
                {
                    return ip.ToString();
                }
            }
            //throw new Exception("No network adapters with an IPv4 address in the system!");
            return "";
        }

        /// <summary>
        /// Get IP Address from database
        /// </summary>
        public static string GetIPAddressFromDatabase(out int port)
        {
            string ipAddress = "";
            port = 0;
            bool hasPort = false;

            try
            {
                ipAddress = TestResultRepository.GetConfigurationByKey("InterfaceSettingsIP").ConfigurationValue.Replace(" ", "");
                hasPort = int.TryParse(TestResultRepository.GetConfigurationByKey("InterfaceSettingsPortNo").ConfigurationValue, out port);
            }
            catch
            {

            }

            return ipAddress;
        }
    }
}
