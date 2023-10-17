using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rug.Osc;

namespace VerarbeitungTest
{
    internal class OSCReceiver
    {
        //OSC Receiver klasse empfängt in seperaten thread OSC Daten
        private int port;
        static OscReceiver receiver;


        public volatile OscPacket OSCData;

        private static void OSCThread()
        {
            try
            {
                while (receiver.State != OscSocketState.Closed)
                {
                   
                    if (receiver.State == OscSocketState.Connected)
                    {
                        
                        OscPacket packet = receiver.Receive();
                        // Testweise OSC Daten in Konsole Loggen
                        Console.WriteLine(packet.ToString());

                        
                    }
                }
            }
            catch (Exception ex)
            {
                
                if (receiver.State == OscSocketState.Connected)
                {
                    Console.WriteLine("Exception in listen loop");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public OSCReceiver(int port = 9001) {
            this.port = port;
            Thread t = new Thread(new ThreadStart(OSCThread));

            receiver = new OscReceiver(port);
            receiver.Connect();
            t.Start();
        }


    }
}
