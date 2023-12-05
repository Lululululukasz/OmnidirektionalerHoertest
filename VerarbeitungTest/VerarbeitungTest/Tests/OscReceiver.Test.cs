using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Rug.Osc;


namespace VerarbeitungTest
{
    /// <summary>
    /// Test Class for the OscReceiver Class. It Starts Automatically after Creation if the Test is Passed it shoud show "Test -> OscReceiver : OK"
    /// </summary>
    internal class OscReceiverTest
    {
        private OscRouter dummy;
        private OscReceiver receiver;
        private int receivedDebugPacks = 0;
        private int receivedJibberish = 0;
        private bool testDone = false;
        private OscSender sender;
        public OscReceiverTest()
        {
            receiver = new OscReceiver(ReceiveTest,9000);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 9000;
            sender = new OscSender(ip, port);
            sender.Connect();
            for(int i = 0;i < 10; i++)
            {
                sender.Send(new OscMessage("/I1-2/mouse/OSCTESTDATA", 42));
            }
            
        }
        /// <summary>
        /// Test Callback for the OscReceiver Class
        /// </summary>
        public void ReceiveTest(string data)
        {
            //Console.WriteLine(data);
            if(data.CompareTo("/I1-2/mouse/OSCTESTDATA, 42") == 0)
            {
                receivedDebugPacks++;
            }
            else
            {
                receivedJibberish++;
            }
            if(receivedDebugPacks >= 10 && receivedJibberish == 0) {
                Console.WriteLine("Test -> OscReceiver : OK");
                receiver.Close();
                sender.Close();
                testDone = true;

            }
            else if(receivedJibberish > 10 )
            {
                Console.WriteLine("Test -> OscReceiver : FAILED");
                receiver.Close();
                sender.Close();
                testDone = true;
            }
        }
        public bool testComplete()
        {
            return testDone;
        }


        
    }
}
