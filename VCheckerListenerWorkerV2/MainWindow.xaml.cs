using log4net.Config;
using log4net.Core;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using VCheckerListenerWorkerV2.Lib.Models;
using VCheckerListenerWorkerV2.Lib.Logic;
using VCheckerListenerWorkerV2.Lib.Logic.HL7;
using Microsoft.Extensions.Configuration;
using VCheckerListenerWorkerV2.Lib.Object;
using System.Diagnostics.Eventing.Reader;
using VCheckerListenerWorkerV2.Lib.Logic.HL7.V251;
using Microsoft.Extensions.Logging;

namespace VCheckerListenerWorkerV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource source = new CancellationTokenSource();
        SynchronizationContext syncContext = SynchronizationContext.Current;
        //Task task = null;

        VCheckerListenerWorkerV2.Lib.Util.Logger sLogger;
        System.Net.Sockets.Socket sListener = null;

        public String sSystemName = "VCheckViewer Listener";

        public System.Threading.Thread sTask;

        public MainWindow()
        {
            InitializeComponent();

            XmlConfigurator.Configure(log4net.LogManager.GetRepository(Assembly.GetEntryAssembly()),
                                      new FileInfo("log4net.config"));

            sLogger = new VCheckerListenerWorkerV2.Lib.Util.Logger();

            List<ListViewObject> sListingObject = new List<ListViewObject>();
        }

        private void StartListener()
        {
            var configBuilder = Host.CreateApplicationBuilder();

            try
            {
                String sHostIP = configBuilder.Configuration.GetSection("Listener:HostIP").Value;
                int iPortNo = Convert.ToInt32(configBuilder.Configuration.GetSection("Listener:Port").Value);

                System.Net.IPEndPoint sIPEndPoint = System.Net.IPEndPoint.Parse(String.Concat(sHostIP, ":", iPortNo));
                sListener = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                                                        System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

                sListener.Bind(sIPEndPoint);
                sListener.Listen(3);
                //source.Token.Register(sListener.Dispose);
                //source.Token.Register(() => StopListen());
                //source.Token.Register(() =>
                //{
                //    sListener.Close();
                //    sListener.Dispose();
                //});

                syncContext.Send(x => lbStatus.Text = "Online", null);
                syncContext.Send(x => lbStatus.Foreground = new BrushConverter().ConvertFrom("Green") as SolidColorBrush, null);

                while (!source.IsCancellationRequested)
                {
                    while (true)
                    {
                        System.Net.Sockets.Socket sClient = sListener.Accept();
                        syncContext.Send(x => txtTest.Text = txtTest.Text = txtTest.Text + "Connection Accepted.", null);

                        byte[] bBuffer = new byte[32768];

                        var childSocket = new Thread(() =>
                        {
                            int s = sClient.Receive(bBuffer);

                            String sData = System.Text.Encoding.ASCII.GetString(bBuffer, 0, s);
                            sData = sData.Replace("\u001c", "")
                                         .Replace("\n", "\r");

                            if (!String.IsNullOrEmpty(sData))
                            {
                                ListViewObject sItem = new ListViewObject
                                {
                                    transactionDate = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt"),
                                    transactionMsgFiltered = sData.Substring(0, 200).Trim(),
                                    transactionMsg = sData
                                };
                                
                                syncContext.Send(x => lvIncomingMessage.Items.Insert(0, sItem), null);
                                syncContext.Send(x => txtTest.Text = txtTest.Text = txtTest.Text + sData, null);

                                NHapi.Base.Parser.XMLParser sXMLParser = new NHapi.Base.Parser.DefaultXMLParser();
                                NHapi.Base.Parser.PipeParser sParser = new NHapi.Base.Parser.PipeParser();
                                NHapi.Base.Model.IMessage sIMessage = sParser.Parse(sData.Trim());

                                String sFileName = String.Empty;
                                String sXMLMessage = String.Empty;
                                String sAckMessage = String.Empty;
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
                            }

                            sClient.Close();
                        });

                        childSocket.Start();
                    }
                }

            }
            catch (System.Net.Sockets.SocketException socketEx)
            {
                // todo: WACAll Cancel trigger
                //if (source.IsCancellationRequested)
                //{
                //    syncContext.Send(x => txtTest.Text = txtTest.Text + "Connection Closed.", null);
                //    syncContext.Send(x => lbStatus.Text = "Offline", null);
                //    syncContext.Send(x => lbStatus.Foreground = new BrushConverter().ConvertFrom("Red") as SolidColorBrush, null);

                //    syncContext.Send(x => bdStatusLight.BorderBrush = new BrushConverter().ConvertFrom("Red") as SolidColorBrush, null);
                //    syncContext.Send(x => bdStatusLight.Background = new BrushConverter().ConvertFrom("Red") as SolidColorBrush, null);
                //    syncContext.Send(x => btnStartListener.IsEnabled = true, null);
                //    syncContext.Send(x => btnStopListener.IsEnabled = false, null);
                //}
            }
            catch (Exception ex)
            {
                String sError = "Exception Error : " + ex.ToString();
                syncContext.Send(x => txtTest.Text = txtTest.Text + sError, null);
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //task = Task.Factory.StartNew(StartListener, source.Token);
            //task = Task.Factory.StartNew(StartListener, source.Token);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //sListener.Disconnect(true);
            //sListener.Dispose();
            //source.Cancel();

            //syncContext.Send(x => txtTest.Text = "Connection Closed.", null);
        }

        private async void btnStartListener_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //task = Task.Factory.StartNew(StartListener(source.Token), source.Token);
                //task = Task.Factory.StartNew(StartListener, source.Token);
                sTask = new System.Threading.Thread(StartListener);
                sTask.Start();

                syncContext.Send(x => bdStatusLight.BorderBrush = new BrushConverter().ConvertFrom("Green") as SolidColorBrush, null);
                syncContext.Send(x => bdStatusLight.Background = new BrushConverter().ConvertFrom("Green") as SolidColorBrush, null);
                syncContext.Send(x => btnStartListener.IsEnabled = false, null);
                syncContext.Send(x => btnStopListener.IsEnabled = true, null);
            }
            catch (Exception ex)
            {
                String abc = "xxxx";
            }
            
        }

        private async void btnStopListener_Click(object sender, RoutedEventArgs e)
        {
            sListener.Close();
            sListener.Dispose();

            sTask.Interrupt();

            syncContext.Send(x => txtTest.Text = txtTest.Text + "Cancellation requested", null);


            syncContext.Send(x => txtTest.Text = txtTest.Text + "Connection Closed.", null);
            syncContext.Send(x => lbStatus.Text = "Offline", null);
            syncContext.Send(x => lbStatus.Foreground = new BrushConverter().ConvertFrom("Red") as SolidColorBrush, null);

            syncContext.Send(x => bdStatusLight.BorderBrush = new BrushConverter().ConvertFrom("Red") as SolidColorBrush, null);
            syncContext.Send(x => bdStatusLight.Background = new BrushConverter().ConvertFrom("Red") as SolidColorBrush, null);
            syncContext.Send(x => btnStartListener.IsEnabled = true, null);
            syncContext.Send(x => btnStopListener.IsEnabled = false, null);
        }

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
                sLogger.Error("Function OutputMessage >>> " + ex.ToString());
            }

        }
    }
}