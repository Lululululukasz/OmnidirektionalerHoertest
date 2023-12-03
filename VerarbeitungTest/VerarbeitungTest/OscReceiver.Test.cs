using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    internal class OscReceiverTest
    {
        private OscRouter dummy;
        private OscReceiver receiver;
        private int receivedDebugPacks = 0;
        private int receivedJibberish = 0;
        private bool testDone = false;
        public OscReceiverTest()
        {
            receiver = new OscReceiver(ReceiveTest,9000);
        }

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
                testDone = true;

            }
            else if(receivedJibberish > 10 )
            {
                Console.WriteLine("Test -> OscReceiver : FAILED");
                receiver.Close();
                testDone = true;
            }
        }
        public bool testComplete()
        {
            return testDone;
        }


        
    }
}
