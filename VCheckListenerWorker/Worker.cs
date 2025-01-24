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

namespace VCheckListenerWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        System.Net.Sockets.Socket sListener;
        VCheckListenerWorker.Lib.Util.Logger sLogger;

        public String sSystemName = "VCheckViewer Listener";

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
            sLogger = new VCheckListenerWorker.Lib.Util.Logger();

            var configBuilder = Host.CreateApplicationBuilder();

            while (!stoppingToken.IsCancellationRequested)
            {
                while (true)
                {
                    System.Net.Sockets.Socket sClient = sListener.Accept();
                    Console.WriteLine("Connection Accepted.");

                    //byte[] bBuffer = new byte[4096];
                    byte[] bBuffer = new byte[32768];

                    var childSocket = new Thread(() =>
                    {
                        int s = sClient.Receive(bBuffer);
                        Console.WriteLine("Received Data.");

                        String sData = System.Text.Encoding.ASCII.GetString(bBuffer, 0, s);
                        sData = sData.Replace("\u001c", "")
                                     .Replace("\n", "\r");

                        if (!String.IsNullOrEmpty(sData))
                        {
                            Console.WriteLine(sData);

                            NHapi.Base.Parser.XMLParser sXMLParser = new NHapi.Base.Parser.DefaultXMLParser();
                            NHapi.Base.Parser.PipeParser sParser = new NHapi.Base.Parser.PipeParser();
                            NHapi.Base.Model.IMessage sIMessage = sParser.Parse(sData.Trim());

                            String sXMLMessage = String.Empty;
                            String sAckMessage = String.Empty;
                            String sFileName = String.Empty;
                            if (sIMessage != null)
                            {
                                sAckMessage = SendAckMessage(sIMessage);
                                var sMessageByte = System.Text.Encoding.UTF8.GetBytes(sAckMessage);
                                sClient.SendAsync(sMessageByte, System.Net.Sockets.SocketFlags.None);

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
            try
            {
                Console.WriteLine("Start Listener connection");

                var builder = Host.CreateApplicationBuilder();
                String sHostIP = builder.Configuration.GetSection("Listener:HostIP").Value;
                int iPortNo = Convert.ToInt32(builder.Configuration.GetSection("Listener:Port").Value);

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
            catch (Exception ex)
            {
                _logger.LogError("StartAsync >>> " + ex.ToString());
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
                    Lib.Logic.HL7.V26.HL7Repository.ProcessMessage(sIMessage, sSystemName);
                    break;

                case "2.5.1":
                    Lib.Logic.HL7.V251.HL7Repository.ProcessMessage(sIMessage, sSystemName);
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
                    sMessage = Lib.Logic.HL7.V251.AckRepository.GenerateAcknowlegeMessage(sIMessage);
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
    }
}
