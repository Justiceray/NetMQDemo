using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;


namespace NetMQReply
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var responseSocket = new ResponseSocket("tcp://*:5555"))
                {

                    var message = responseSocket.ReceiveFrameString();

                    Console.WriteLine("responseSocket : Server Received '{0}'", message);

                    Console.WriteLine("responseSocket Sending 'World'");

                    responseSocket.SendFrame("World");

                    Console.WriteLine("requestSocket : Received '{0}'", message);

                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
