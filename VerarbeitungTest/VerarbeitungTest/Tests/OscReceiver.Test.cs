using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rug.Osc;


namespace VerarbeitungTest
{
    /// <summary>
    /// Test Class for the OscReceiver Class. It Starts Automatically after Creation if the Test is Passed it shoud show "Test -> OscReceiver : OK"
    /// </summary>

    [TestFixture]

    internal class OscReceiverTest
    {
        private int receivedDebugPacks = 0;
        private int receivedJibberish = 0;

        private OscSender sender;

        private void Callback(string data)
        {
            if (data.CompareTo("/I1-2/mouse/OSCTESTDATA, 42") == 0)
            {
                receivedDebugPacks++;
            }
            else
            {
                receivedJibberish++;
            }
        }

        [Test]
        public void OscReceiveTest()
        {
            OscReceiver receiver = new OscReceiver(Callback, 9000);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 9000;
            sender = new OscSender(ip, port);
            sender.Connect();
            for (int i = 0; i < 10; i++)
            {
                sender.Send(new OscMessage("/I1-2/mouse/OSCTESTDATA", 42));
            }

            Thread.Sleep(1000);

            Assert.Multiple(() => {
                Assert.That(receivedDebugPacks, Is.EqualTo(10));
                Assert.That(receivedJibberish, Is.Zero);
            });
            sender.Close();
        }
    }
}