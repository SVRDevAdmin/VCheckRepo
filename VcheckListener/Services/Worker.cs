using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using VCheck.Interface.API;
using VCheck.Lib.Data.Models;
using VCheckListener.Lib.Logics;
using VCheckListener.Lib.Models;

namespace VCheckListener.Services
{
    class Worker
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public Action<string> OnClientConnect; // callback to UI
        public Action<string> OnDataReceived;
        public Action<string> OnClientDisconnect;
        public Action<string> OnError;


        public static System.Net.Sockets.Socket sListener;
        public String sSystemName = "VCheckViewer Listener";
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        public static MainModel MainModel { get; } = new MainModel();
        private string sDeviceType = "";
        private mst_configuration sClinicID;
        private Lib.Models.DeviceModel sDevice;
        public string IpPort;

        public void Start(string sIPAddress, int sPort)
        {

            System.Net.IPEndPoint sIPEndPoint = System.Net.IPEndPoint.Parse(String.Concat(sIPAddress, ":", sPort));

            sListener = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                                                                                System.Net.Sockets.SocketType.Stream,
                                                                                System.Net.Sockets.ProtocolType.Tcp);
            sListener.Bind(sIPEndPoint);
            sListener.Listen(backlog: 10);

            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    if (sListener.Poll(100000, SelectMode.SelectRead))
                    {
                        Socket sClient = sListener.Accept();

                        IPEndPoint remoteIpEndPoint = sClient.RemoteEndPoint as IPEndPoint;

                        IpPort = remoteIpEndPoint.Address.ToString() + ":" + remoteIpEndPoint.Port;
                        sDevice = TestResultRepository.GetDeviceByIPSerialNo(remoteIpEndPoint.Address.ToString(), null, out sDeviceType);
                        sClinicID = TestResultRepository.GetConfigurationByKey("ClinicID");
                        MainModel.DeviceSerialNum = sDevice != null && sDevice.id != 0 ? sDevice.DeviceName : null;
                        MainModel.DeviceType = sDeviceType;

                        Task.Run(() => ProcessMessage(sClient, IpPort));

                        if (sDeviceType == "H6")
                        {
                            Task.Run(() => ProcessSchedule(sClient));
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                log.Error("Socket Error: ", ex);
                OnError?.Invoke("Socket error occur.");
            }
            catch (Exception ex)
            {
                log.Error("Start Error: ", ex);
                OnError?.Invoke("Error occur.");
            }
            finally
            {
                sListener.Dispose();
            }
            
        }

        private async Task ProcessMessage(Socket sClient, string sIpPort)
        {
            string tempData = "";
            bool continueLoop = true;
            OnClientConnect?.Invoke(sIpPort); // 🔔 trigger balloon tip

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

                if (!isConnected) { sClient.Close(); break; }

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
                    Thread.Sleep(500);
                }

                if (!String.IsNullOrEmpty(sData) && sData != tempData)
                {
                    tempData = sData;

                    OnDataReceived?.Invoke(sData.Trim());
                    OutputMessage(sData.Trim(), null, null);

                    NHapi.Base.Parser.XMLParser sXMLParser = new NHapi.Base.Parser.DefaultXMLParser();
                    NHapi.Base.Parser.PipeParser sParser = new NHapi.Base.Parser.PipeParser() { ValidationContext = new CustomMessageValidation() };
                    NHapi.Base.Model.IMessage sIMessage = null;
                    bool continueProcessData = false;

                    try
                    {
                        if ((sData.Contains("ORU^R01") && sData.Contains("OBX")) || !sData.Contains("ORU^R01"))
                        {
                            continueProcessData = true;
                        }

                        sIMessage = sParser.Parse(sData.Trim());
                    }
                    catch (Exception ex)
                    {
                        log.Error("Message failed during parsing: ", ex);
                        OnError?.Invoke("Error parsing data.");
                    }

                    String sXMLMessage = String.Empty;
                    String sAckMessage = String.Empty;
                    String sFileName = "TestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                    if (sIMessage != null)
                    {
                        sAckMessage = await SendAckMessage(sIMessage);
                        var sMessageByte = Encoding.UTF8.GetBytes(sAckMessage);
                        sClient.Send(sMessageByte, SocketFlags.None);

                        //if (sDeviceType != "H6" && sDeviceType != "U3")
                        //{
                        //    sClient.Close();
                        //    continueLoop = false;
                        //}

                        //var processed = ProcessIMessage(sIMessage, sSystemName);

                        //if (!processed)
                        //{
                        //    OnError?.Invoke("Failed to process data.");
                        //}

                        if (continueProcessData)
                        {
                            //Task.Run(() => ProcessIMessage(sIMessage, sSystemName));

                            sXMLMessage = sXMLParser.Encode(sIMessage);
                            OutputMessage(null, sXMLMessage, sAckMessage);
                        }
                    }
                }
            }

            OnClientDisconnect?.Invoke(sIpPort);
        }

        private async Task ProcessSchedule(Socket client)
        {
            VCheckAPI vCheckAPI = new VCheckAPI();

            while (sClinicID != null)
            {
                var schedulesString = await vCheckAPI.GetScheduleListNotSent(sClinicID.ConfigurationValue);
                var schedulesExtended = string.IsNullOrEmpty(schedulesString) ? new List<ScheduledTestModelExtended>() : JsonConvert.DeserializeObject<List<ScheduledTestModelExtended>>(schedulesString);

                if (schedulesExtended.Count > 0)
                {
                    schedulesExtended = schedulesExtended.Where(x => x.IDAnalyzers.FirstOrDefault().Analyzers.Contains(sDeviceType)).ToList();
                    var order = await Lib.Logics.HL7.V231.OrderRepository.GenerateOrderMessageORM(schedulesExtended);

                    if (!string.IsNullOrEmpty(order))
                    {
                        var testsMessageByte = Encoding.UTF8.GetBytes(order);
                        client.SendAsync(testsMessageByte, SocketFlags.None);

                        foreach (var schedule in schedulesExtended)
                        {
                            await vCheckAPI.UpdateScheduleStatus(schedule.Schedule.LocationID, schedule.Schedule.PatientID, schedule.Schedule.ScheduleUniqueID.Split("-")[1], schedule.Schedule.CreatedBy, 1);
                            await vCheckAPI.UpdateScheduleAnalyzer(sDevice.DeviceName, schedule.Schedule.ScheduleUniqueID);
                        }
                    }

                }

                Thread.Sleep(5000);
            }
        }

        public void Stop()
        {
            _cts?.Cancel();
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
                    Lib.Logics.HL7.V26.HL7Repository.ProcessMessageAsync(sIMessage, sSystemName);
                    break;

                case "2.5.1":
                    Lib.Logics.HL7.V251.HL7Repository.ProcessMessage(sIMessage, sSystemName);
                    break;

                case "2.3.1":
                    if (sIMessage.Message.Message.GetType() == typeof(NHapi.Model.V231.Message.ORU_R01))
                    {
                        Lib.Logics.HL7.V231.HL7Repository.ProcessMessageAsync(sIMessage, sSystemName);
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
                    sMessage = Lib.Logics.HL7.V26.AckRepository.GenerateAcknowlegeMessage(sIMessage);

                    break;

                case "2.5.1":
                    if (sIMessage.Message.Message.GetType() == typeof(NHapi.Model.V251.Message.QBP_Q11))
                    {
                        sMessage = Lib.Logics.HL7.V251.AckRepository.GenerateAcknowlegeMessageQBP(sIMessage);
                    }
                    else if (sIMessage.Message.Message.GetType() == typeof(NHapi.Model.V251.Message.OUL_R22))
                    {
                        sMessage = Lib.Logics.HL7.V251.AckRepository.GenerateAcknowlegeMessage(sIMessage);
                    }

                    break;

                case "2.3.1":
                    if (sIMessage.Message.Message.GetType() == typeof(NHapi.Model.V231.Message.ORU_R01))
                    {
                        sMessage = Lib.Logics.HL7.V231.AckRepository.GenerateAcknowlegeMessageORU(sIMessage);
                    }
                    else if (sIMessage.Message.Message.GetType() == typeof(NHapi.Model.V231.Message.QRY_Q02))
                    {
                        Lib.Logics.HL7.V231.AckRepository ackRepository = new Lib.Logics.HL7.V231.AckRepository();
                        sMessage = await ackRepository.GenerateAcknowlegeMessageQRY(sIMessage);
                    }
                    break;

                default:
                    break;
            }

            return sMessage;
        }

        public void OutputMessage(String sData, String sXMLMessage, String sAckMessage)
        {
            try
            {
                var sBuilder = Host.CreateApplicationBuilder();

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
                    File.WriteAllText(outputPathHL7 + "TestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".hl7", sData, System.Text.Encoding.ASCII);
                }

                // Output to XML file
                if (!String.IsNullOrEmpty(sOutputPathXML))
                {
                    String outputPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathXML);
                    if (!Directory.Exists(outputPath))
                    {
                        Directory.CreateDirectory(outputPath);
                    }
                    File.WriteAllText(outputPath + "TestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml", sXMLMessage, System.Text.Encoding.ASCII);
                }

                if (!String.IsNullOrEmpty(sOutputPathACK))
                {
                    String outputPathACK = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), sOutputPathACK);
                    if (!Directory.Exists(outputPathACK))
                    {
                        Directory.CreateDirectory(outputPathACK);
                    }
                    File.WriteAllText(outputPathACK + "TestResult_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".hl7", sAckMessage, System.Text.Encoding.ASCII);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client error: " + ex.Message);
            }

        }
    }
}
