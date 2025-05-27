using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using VCheck.Lib.Data.Models;
using VCheck.Lib.Data;
using System.Net;
using ScheduleAutomation.Lib.Logic;
using VCheck.Interface.API;

namespace ScheduleAutomation.Lib.Services
{
    public class HL7
    {
        static TcpClient ourTcpClient = null;
        static NetworkStream networkStream = null;
        static String receivedMessage = "";

        public static async Task SendMessage(ScheduledTestModel scheduleTest, DeviceModel device)
        {
            String sContent = MessageGenerator.GenerateOMLO33Message(scheduleTest);
            int timeout = 6000;

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
                            VCheckAPI vCheckAPI = new VCheckAPI();
                            var test = await vCheckAPI.UpdateScheduleStatus(scheduleTest.LocationID, scheduleTest.PatientID, scheduleTest.ScheduleUniqueID.Split("-")[1], scheduleTest.CreatedBy, 1);
                        }

                    }
                    catch (TaskCanceledException cancelled)
                    {

                    }
                    catch (Exception ex)
                    {
                        
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

        private static bool ProcessAckMessage(string message)
        {
            int ix = message.IndexOf("MSA|");
            var statusCode = message.Substring(ix + 4, 2);

            if (statusCode == "AA")
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
