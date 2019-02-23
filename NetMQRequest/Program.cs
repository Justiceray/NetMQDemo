using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMQRequest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var requestSocket = new RequestSocket("tcp://localhost:5555"))
            {
        
                Console.WriteLine("requestSocket : Sending 'Hello'");
                requestSocket.SendFrame("Hello");

                var message = requestSocket.ReceiveFrameString();
                Console.WriteLine("requestSocket : Received '{0}'", message);

                Console.ReadLine();
            }
        }
    }
}
