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

        static Object _lock;

        private Test testCallback;

        public static List<OscPacket> Packages;

        private static void OSCThread()//seperater Thread zum Empfangen von OSC Daten
        {
            try
            {
                while (receiver.State != OscSocketState.Closed)
                {
                   
                    if (receiver.State == OscSocketState.Connected)
                    {
                        
                        OscPacket packet = receiver.Receive();
                        // Testweise OSC Daten in Konsole Loggen
                        //Console.WriteLine(packet.ToString());

                        //ThreadSafe packet zur liste hinzufügen
                        lock(_lock)
                        {
                            Packages.Add(packet);
                        }
                        
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

        public OSCReceiver(Test callback,int port = 9000) {
            this.port = port;
            Thread t = new Thread(new ThreadStart(OSCThread));
            Packages = new List<OscPacket>();
            _lock = new Object();
            receiver = new OscReceiver(port);
            receiver.Connect();
            t.Start();
        }

        public void ClearBuffer()
        {
            Packages.Clear();
        }
    }
}
