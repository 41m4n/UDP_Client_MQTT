using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UDP_Client_MQTT
{
    class Program
    {
        static private readonly object mReadLock = new object();
        static void Main(string[] args)
        {
            //=============================================================UDPER
            //UDPer udp = new UDPer();
            //udp.Start();

            //ConsoleKeyInfo cki;
            //do
            //{
            //    if (Console.KeyAvailable)
            //    {
            //        cki = Console.ReadKey(true);
            //        switch (cki.KeyChar)
            //        {
            //            case 's':
            //                udp.Send(new Random().Next().ToString());
            //                break;
            //            case 'x':
            //                udp.Stop();
            //                return;
            //        }
            //    }
            //    Thread.Sleep(10);
            //} while (true);
            //=============================================================UDPER


            //=============================================================UDPSocket
            //UDPSocket c = new UDPSocket();
            //c.Client("127.0.0.1", 27000);
            //c.Send("GetIP");

            //Console.ReadKey();
            //=============================================================UDPSocket


            //=============================================================UDPClient
            int PORT = 27000;
            UdpClient udpClient = new UdpClient();
            //udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));

            var from = new IPEndPoint(0, 0);
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var recvBuffer = udpClient.Receive(ref from);

                        processMessage(Encoding.UTF8.GetString(recvBuffer), from);
                        Console.WriteLine("Received Message:" + Encoding.UTF8.GetString(recvBuffer));
                    }
                    catch (Exception ex) {
                        Console.WriteLine("Error In Receive Message:" + ex.Message);
                    }

                    
                }
            });

            //ConsoleKeyInfo cki;
            //do
            //{
            //    if (Console.KeyAvailable)
            //    {
            //        cki = Console.ReadKey(true);
            //        switch (cki.KeyChar)
            //        {
            //            case 's':
            //                var data = Encoding.UTF8.GetBytes("GetIP");
            //                udpClient.Send(data, data.Length, "255.255.255.255", PORT);
            //                //udp.Send(new Random().Next().ToString());
            //                break;
            //            case 'x':
            //                //udp.Stop();
            //                return;
            //        }
            //    }
            //    Thread.Sleep(10);
            //} while (true);
            var data = Encoding.UTF8.GetBytes("GetIP");
            udpClient.Send(data, data.Length, "255.255.255.255", PORT);

            //=============================================================UDPClient


            Console.ReadKey();
        }

        public static void processMessage(string message, IPEndPoint targetUdp)
        {
            JObject o = new JObject();

            try
            {
                o = JObject.Parse(message);
                //Console.WriteLine("Received From Server:" + message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Parsing JSON");
                return;
            }

            if (o != null)
            {
                Console.WriteLine("JObject is not null");
                string receivedIp = o["ServerIP"].Value<string>();
                Console.WriteLine("ReceivedIP:" + receivedIp);
            }
            else
            {
                Console.WriteLine("JObject is null");
            }
        }
    }
}
