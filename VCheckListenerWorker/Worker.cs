using Mysqlx.Crud;
using NHapiTools.Model.V26.Segment;
using System.Reflection;
using System.IO;
using VCheckListenerWorker.Lib.Logic;
using VCheckListenerWorker.Lib.Models;
using log4net.Config;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NHapi.Model.V23.Segment;
using Org.BouncyCastle.Asn1;
using System.Net.Sockets;
using System.Net;
using NHapi.Base.Validation.Implementation;
using NHapi.Base.Util;
using NHapi.Base.Parser;
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
            XmlConfigurator.Configure(log4net.LogManager.GetRepository(Assembly.GetEntryAssembly()),
                                      new FileInfo("log4Net.config"));
            sLogger = new Lib.Util.Logger();

            var configBuilder = Host.CreateApplicationBuilder();

            while (!stoppingToken.IsCancellationRequested)
            {
                while (true)
                {
                    System.Net.Sockets.Socket sClient = sListener.Accept();

                    // Get the client's IP address
                    IPEndPoint remoteIpEndPoint = sClient.RemoteEndPoint as IPEndPoint;

                    string clientIpAddress = remoteIpEndPoint.Address.ToString();
                    string clientIpPort = remoteIpEndPoint.Port.ToString();

                    var device = TestResultRepository.GetDeviceByIPSerialNo(clientIpAddress, null);

                    MainModel.DeviceSerialNum = device != null && device.id != 0 ? device.DeviceName : null;

                    //Console.WriteLine("Connection Accepted. Client IP Address is " + clientIpAddress + ":" + clientIpPort);

                    int byteLength = sClient.Available;
                    while (true)
                    {
                        Thread.Sleep(1000);
                        var temp = sClient.Available;
                        if (temp == byteLength && temp != 0) { break; }
                        byteLength = temp;
                    }

                    //byte[] bBuffer = new byte[1048576];
                    //byte[] bBuffer = new byte[32768];
                    byte[] bBuffer = new byte[byteLength];

                    var childSocket = new Thread(() =>
                    {
                        int s = sClient.Receive(bBuffer);
                        String sDataTemp = System.Text.Encoding.ASCII.GetString(bBuffer, 0, s);
                        sDataTemp = sDataTemp.Replace("\u001c", "")
                                     .Replace("\n", "\r");
                        String sData = sDataTemp;

                        while (sClient.Available != 0)
                        {
                            s = sClient.Receive(bBuffer);
                            sDataTemp = System.Text.Encoding.ASCII.GetString(bBuffer, 0, s);
                            sDataTemp = sDataTemp.Replace("\u001c", "")
                                         .Replace("\n", "\r");
                            if (string.IsNullOrEmpty(sDataTemp) || sDataTemp.Contains("MSH|")) { break; }
                            sData += sDataTemp;
                        }

                        if (!String.IsNullOrEmpty(sData))
                        {

                            Console.WriteLine("Data Message >> ");
                            Console.WriteLine(sData);

                            NHapi.Base.Parser.XMLParser sXMLParser = new NHapi.Base.Parser.DefaultXMLParser();
                            //NHapi.Base.Parser.PipeParser sParser = new NHapi.Base.Parser.PipeParser();
                            //NHapi.Base.Parser.ParserOptions sParserOptions = new ParserOptions { InvalidObx2Type = "ED" };
                            NHapi.Base.Parser.PipeParser sParser = new NHapi.Base.Parser.PipeParser() { ValidationContext = new CustomMessageValidation() };
                            //NHapi.Base.Parser.PipeParser sParser = new NHapi.Base.Parser.PipeParser() { ValidationContext = new StrictValidation() };
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
                                sAckMessage = SendAckMessage(sIMessage);
                                //var trimmedAckMessage = sAckMessage.Trim();
                                var sMessageByte = System.Text.Encoding.UTF8.GetBytes(sAckMessage);
                                sClient.SendAsync(sMessageByte, System.Net.Sockets.SocketFlags.None);

                                Console.WriteLine("");
                                Console.WriteLine("Acknowledge Message >> ");
                                Console.WriteLine(sAckMessage);

                                ProcessIMessage(sIMessage, sSystemName);

                                sFileName = "TestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                                sXMLMessage = sXMLParser.Encode(sIMessage);

                                OutputMessage(configBuilder, sFileName, sData, sXMLMessage, sAckMessage);
                            }

                            Console.WriteLine("---------------------------------------------------------------------------------");
                        }

                        sClient.Close();
                    });

                    childSocket.Start();
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
                    else if (sIMessage.Message.Message.GetType() == typeof(NHapi.Model.V231.Message.QRY_Q02))
                    {

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
        private String SendAckMessage(NHapi.Base.Model.IMessage sIMessage)
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
                        sMessage = Lib.Logic.HL7.V231.AckRepository.GenerateAcknowlegeMessageQRY(sIMessage);
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
