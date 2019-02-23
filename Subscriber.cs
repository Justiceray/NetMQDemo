using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace NetMQ
{
    public class Subscriber
    {
        public static IList<string> allowableCommandLineArgs
        = new[] { "TopicA", "TopicB", "All" };

        public Subscriber()
        {

        }
    }
}
