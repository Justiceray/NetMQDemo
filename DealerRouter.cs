using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
namespace NetMQ
{
    public class DealerRouter
    {

        public DealerRouter()
        {
            const int delay = 3000;

            //Keep dealersocket reference within each three threads
            var clientSocketPerThread = new ThreadLocal<DealerSocket>();


            using (var server = new RouterSocket("tcp://127.0.0.1:5556"))
            using (var poller = new NetMQPoller())
            {
                for (int i = 0; i < 3; i++)
                {
                    Task.Factory.StartNew(state =>
                    {
                        DealerSocket client = null;

                        if (!clientSocketPerThread.IsValueCreated)
                        {
                            client = new DealerSocket();
                            client.Options.Identity = Encoding.Unicode.GetBytes(state.ToString());
                            client.Connect("tcp://127.0.0.1:5556");
                            client.ReceiveReady += Client_ReceivedReady;
                            clientSocketPerThread.Value = client;
                            poller.Add(client);   



                        }
                        else
                        {
                            client = clientSocketPerThread.Value;
                        }

                        //the dealersocket will provide an identity for message when it is created
                        while (true)
                        {
                            var messageToServer = new NetMQMessage();
                            messageToServer.AppendEmptyFrame();
                            messageToServer.Append(state.ToString());
                            Console.WriteLine("======================================");
                            Console.WriteLine(" OUTGOING MESSAGE TO SERVER ");
                            Console.WriteLine("======================================");
                            PrintFrames("Client Sending", messageToServer);
                            client.SendMultipartMessage(messageToServer);
                            Thread.Sleep(delay);
                        }
                    }, string.Format("client {0}", i), TaskCreationOptions.LongRunning);
                }
                poller.RunAsync();

                while (true)
                {
                    var clientMessage = server.ReceiveMultipartMessage();
                    Console.WriteLine("======================================");
                    Console.WriteLine(" INCOMING CLIENT MESSAGE FROM CLIENT ");
                    Console.WriteLine("======================================");
                    PrintFrames("Server receiving", clientMessage);

                    if (clientMessage.FrameCount == 3)
                    {
                        var clientAddress = clientMessage[0];
                        var clientOriginalMessage = clientMessage[2].ConvertToString();
                        string response = string.Format("{0} back from server {1} address {2}",
                            clientOriginalMessage, DateTime.Now.ToLongTimeString(),clientAddress.ConvertToString());
                        var messageToClient = new NetMQMessage();
                        messageToClient.Append(clientAddress);
                        messageToClient.AppendEmptyFrame();
                        messageToClient.Append(response);
                        server.SendMultipartMessage(messageToClient);
                    }
                }

            }
        }

        private void PrintFrames(string operationType, NetMQMessage message)
        {
            for (int i = 0; i < message.FrameCount; i++)
            {
                Console.WriteLine("{0} Socket: Frame[{1}] = {2}", operationType, i, message[i].ConvertToString());
            }
        }

        private void Client_ReceivedReady(object sender, NetMQSocketEventArgs e)
        {
            bool hasMore = false;
            e.Socket.ReceiveFrameString(out hasMore);

            if (hasMore)
            {
                string message = e.Socket.ReceiveFrameString(out hasMore);
                Console.WriteLine("Reply:{0}", message);
            }
        }
    }
}
