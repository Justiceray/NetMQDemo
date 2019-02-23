using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace NetMQ
{
    public class Subscriber
    {

        public Subscriber()
        {

        }

        public void Subscribe()
        {
            using (SubscriberSocket subscriber = new SubscriberSocket())
            {
                subscriber.Options.ReceiveHighWatermark = 1000;
               
                subscriber.Connect("tcp://127.0.0.1:12345");
                subscriber.Subscribe("TopicA");

      
                var data = subscriber.ReceiveFrameString();
                Console.WriteLine("Topic: TopicA received data: {0}", data);

            }
        }
    }
}
