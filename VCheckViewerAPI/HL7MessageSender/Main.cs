using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Net.Sockets;
using System.Text;
using VCheckViewerAPI.Message.CreateScheduledTest;

namespace VCheckViewerAPI.HL7MessageSender
{

    public class Main
    {
        TcpClient ourTcpClient = null;
        NetworkStream networkStream = null;
        String receivedMessage = "";

        public void SendMessage(CreateScheduledDataRequestBody info)
        {
            String sContent = MessageGenerator.GenerateOMLO33Message(info);

            try
            {
                ourTcpClient = new TcpClient();
                ourTcpClient.Connect(IPAddress.Parse("192.168.0.205"), 5067); // C10
                Console.WriteLine("Connected to server....");
                Console.WriteLine("");

                //get the IO stream on this connection to write to
                networkStream = ourTcpClient.GetStream();
                var sendMessageByteBuffer = Encoding.UTF8.GetBytes(sContent);

                if (networkStream.CanWrite)
                {
                    networkStream.Write(sendMessageByteBuffer, 0, sendMessageByteBuffer.Length);
                    byte[] receiveMessageByteBuffer = new byte[ourTcpClient.ReceiveBufferSize];
                    var bytesReceivedFromServer = networkStream.Read(receiveMessageByteBuffer, 0, receiveMessageByteBuffer.Length);

                    if (bytesReceivedFromServer > 0 && networkStream.CanRead)
                    {
                        receivedMessage = Encoding.UTF8.GetString(receiveMessageByteBuffer);
                    }

                    var message = receivedMessage.Replace("\0", string.Empty);
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
    }
}
