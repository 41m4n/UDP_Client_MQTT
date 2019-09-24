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
    public class UDPer
    {
        const int PORT_NUMBER = 27000;

        Thread t = null;
        public void Start()
        {
            if (t != null)
            {
                throw new Exception("Already started, stop first");
            }
            Console.WriteLine("Started listening");
            //StartListening();
        }
        public void Stop()
        {
            try
            {
                udp.Close();
                Console.WriteLine("Stopped listening");
            }
            catch { /* don't care */ }
        }

        private readonly UdpClient udp = new UdpClient(PORT_NUMBER);
        IAsyncResult ar_ = null;

        private void StartListening()
        {
            ar_ = udp.BeginReceive(Receive, new object());
        }
        private void Receive(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
            byte[] bytes = udp.EndReceive(ar, ref ip);
            string message = Encoding.ASCII.GetString(bytes);
            Console.WriteLine("Received Message:" + message);
            JObject o = JObject.Parse(message);
            if (o != null)
            {
                Console.WriteLine("JObject is not null");
                string receivedIp = o["ServerIP"].Value<string>();
                Console.WriteLine("ReceivedIP:" + receivedIp);
            }
            else {
                Console.WriteLine("JObject is null");
            }
            //Console.WriteLine("From {0} received: {1} ", ip.Address.ToString(), message);
            //StartListening();
        }
        public void Send(string message)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), PORT_NUMBER);
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            client.Send(bytes, bytes.Length, ip);
            StartListening();
            client.Close();
            Console.WriteLine("Sent: {0} ", message);
        }
    }
}
