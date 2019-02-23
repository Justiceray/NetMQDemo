using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Worker
{
    class Worker
    {
        static void Main(string[] args)
        {
            // Task Worker
            // Connects PULL socket to tcp://localhost:5557
            // collects workload for socket from Ventilator via that socket
            // Connects PUSH socket to tcp://localhost:5558
            // Sends results to Sink via that socket
            Console.WriteLine("====== WORKER ======");

            using (var receiver = new PullSocket())
            using (var sender = new PushSocket())
            {
                receiver.Connect("tcp://localhost:5558");
                sender.Bind("tcp://localhost:5559");
                //process tasks forever

                int count = 0;
                while (true)
                {
                    count++;
                    //workload from the vetilator is a simple delay
                    //to simulate some work being done, see
                    //Ventilator.csproj Proram.cs for the workload sent
                    //In real life some more meaningful work would be done
                    string workload = receiver.ReceiveFrameString();

                    //simulate some work being done
                    // Thread.Sleep(int.Parse(workload));

                    //send results to sink, sink just needs to know worker
                    //is done, message content is not important, just the presence of
                    //a message means worker is done.
                    //See Sink.csproj Proram.cs
                    Console.WriteLine("receive frame {1} Counts {0}", count, workload);
                    sender.SendFrame(string.Empty);

                }
            }
        }
    }
}
