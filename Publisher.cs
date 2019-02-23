using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

namespace NetMQ
{
    public class Publisher
    {
        public Publisher()
        {
        }

        /// <summary>
        /// Create a publishersocket and send data
        /// </summary>
        public void Publish()
        {
            Random rand = new Random(50);

            using (var pubSocket = new PublisherSocket())
            {
                Console.WriteLine("Publisher socket binding...");
                pubSocket.Options.SendHighWatermark = 1000;
                pubSocket.Bind("tcp://*:12345");


                var msg = "TopicA msg";
                Console.WriteLine("Sending message : {0}", msg);
                pubSocket.SendMoreFrame("TopicA").SendFrame(msg);

            }
        }
    }
}
