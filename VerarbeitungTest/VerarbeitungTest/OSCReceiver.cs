using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    internal class OscReceiver
    {
        private static Rug.Osc.OscReceiver receiver;

        private static Object _lock;
        private static Action<string> callback;
        private Thread netThread;
        public OscReceiver(Action<string> _callback, int port = 9000)
        {
            //Console.WriteLine("Creating Osc Receiver");
            callback = _callback;
            netThread = new Thread(new ThreadStart(OSCThread));
            _lock = new Object();
            receiver = new Rug.Osc.OscReceiver(port);
            receiver.Connect();
            netThread.Start();
            //Console.WriteLine("All init");
        }

        private static void OSCThread()//seperater Thread zum Empfangen von OSC Daten
        {
            try
            {
                while (receiver.State != OscSocketState.Closed)
                {

                    if (receiver.State == OscSocketState.Connected)
                    {

                        OscPacket packet = receiver.Receive();
                        //ThreadSafe packet zur liste hinzufügen
                        lock (_lock)
                        {
                            if(callback != null)
                            {
                                callback(packet.ToString());//callback aufrufen um daten zu übergeben
                            }
                                 
                        }

                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (receiver.State == OscSocketState.Connected)
                {
                    Console.WriteLine("Exception in listen loop");
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine("[i] Network Thread Closed");
        }
        public void Close()
        {
            receiver.Close();
            //netThread.Abort();
        }
    }
}
