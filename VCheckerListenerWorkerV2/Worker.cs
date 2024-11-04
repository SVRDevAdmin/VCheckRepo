using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Hosting;

namespace VCheckerListenerWorkerV2
{
    public class Worker: BackgroundService
    {
        System.Net.Sockets.Socket sListener;

        public Worker()
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                while (true)
                {
                    System.Net.Sockets.Socket sClient = sListener.Accept();
                    Console.WriteLine("Connection Accepted.");

                    byte[] bBuffer = new byte[32768];

                    var childSocket = new Thread(() =>
                    {
                        int s = sClient.Receive(bBuffer);

                        String sData = System.Text.Encoding.ASCII.GetString(bBuffer, 0, s);
                        sData = sData.Replace("\u001c", "")
                                     .Replace("\n", "\r");

                        Console.WriteLine(sData);
                    });
                    childSocket.Start();
                }
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            System.Net.IPEndPoint sIPEndPoint = System.Net.IPEndPoint.Parse("192.168.0.4:8484");
            sListener = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
                                                        System.Net.Sockets.SocketType.Stream,
                                                        System.Net.Sockets.ProtocolType.Tcp);
            sListener.Bind(sIPEndPoint);
            sListener.Listen(3);

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            sListener.Disconnect(true);
            sListener.Dispose();

            return base.StopAsync(cancellationToken);
        }
    }
}
