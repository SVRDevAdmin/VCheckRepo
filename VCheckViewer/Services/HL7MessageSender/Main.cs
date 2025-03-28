using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using VCheck.Lib.Data;
using VCheck.Lib.Data.Models;
using VCheckViewer;
using VCheckViewer.Lib.Function;

namespace VCheckViewerAPI.HL7MessageSender
{

    public class Main
    {
        TcpClient ourTcpClient = null;
        NetworkStream networkStream = null;
        String receivedMessage = "";

        public async Task SendMessage(ScheduledTestModel scheduleTest)
        {
            DeviceModel device = DeviceRepository.GetDeviceByID(App.AnalyzerID, ConfigSettings.GetConfigurationSettings());
            String sContent = MessageGenerator.GenerateOMLO33Message(scheduleTest);
            int timeout = 60000;

            //String sContent = "";
            //int timeout = 5000;

            bool success = false;

            try
            {
                using (var cts = new CancellationTokenSource())
                {
                    cts.CancelAfter(timeout);

                    try
                    {
                        var ipAddress = device.DeviceIPAddress.Replace(" ", "");

                        ourTcpClient = new TcpClient();
                        await ourTcpClient.ConnectAsync(IPAddress.Parse(ipAddress), 5067).WaitAsync(cts.Token);

                        //get the IO stream on this connection to write to
                        networkStream = ourTcpClient.GetStream();
                        var sendMessageByteBuffer = Encoding.UTF8.GetBytes(sContent);

                        if (networkStream.CanWrite)
                        {
                            networkStream.Write(sendMessageByteBuffer, 0, sendMessageByteBuffer.Length);
                            byte[] receiveMessageByteBuffer = new byte[ourTcpClient.ReceiveBufferSize];
                            var bytesReceivedFromServer = await networkStream.ReadAsync(receiveMessageByteBuffer, 0, receiveMessageByteBuffer.Length).WaitAsync(cts.Token);

                            if (bytesReceivedFromServer > 0 && networkStream.CanRead)
                            {
                                receivedMessage = Encoding.UTF8.GetString(receiveMessageByteBuffer);

                                var message = receivedMessage.Replace("\0", string.Empty);

                                success = ProcessAckMessage(message);
                            }
                        }

                        if (success)
                        {
                            App.MainViewModel.Origin = "SentToAnalyzer";
                        }
                        else
                        {
                            App.MainViewModel.Origin = "FailedToSendToAnalyzer";
                        }

                    }
                    catch (TaskCanceledException cancelled)
                    {
                        App.MainViewModel.Origin = "CanceledSendToAnalyzer";
                    }
                    catch (Exception ex)
                    {
                        App.MainViewModel.Origin = "FailedToSendToAnalyzer";
                    }

                }
            }
            catch (Exception ex)
            {
                //display any exceptions that occur to console
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //close the IO strem and the TCP connection
                networkStream?.Close();
                ourTcpClient?.Close();
            }
        }

        private bool ProcessAckMessage(string message)
        {
            int ix = message.IndexOf("MSA|");
            var statusCode = message.Substring(ix + 4, 2);

            if(statusCode == "AA")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
