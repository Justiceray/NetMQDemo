﻿using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            // PubSubDemo();

            DealerRouter dealerRouter = new DealerRouter();


        }


       /// <summary>
       /// Request Reply Demo
       /// </summary>
       private void RequestResponseDemo()
        {
            using (var responseSocket = new ResponseSocket("@tcp://*:5555"))
            using (var requestSocket = new RequestSocket("@tcp://localhost:5555"))
            {
                Console.WriteLine("requestSocket : Sending 'Hello'");
                requestSocket.SendFrame("Hello");

                var message = responseSocket.ReceiveFrameString();

                Console.WriteLine("responseSocket : Server Received '{0}'", message);

                Console.WriteLine("responseSocket Sending 'World'");
                responseSocket.SendFrame("World");

                message = requestSocket.ReceiveFrameString();
                Console.WriteLine("requestSocket : Received '{0}'", message);

                Console.ReadLine();
            }
        }

        private static void PubSubDemo()
        {
            Publisher pub = new Publisher();
            Subscriber sub = new Subscriber();

            while (true)
            {
                pub.Publish();
                sub.Subscribe();
            }
        }
    }
}
