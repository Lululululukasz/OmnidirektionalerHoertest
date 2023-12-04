using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    class Main
    {
        public static void run()
        {
            //Run Unit Tests
            OscReceiverTest test1 = new OscReceiverTest();
            
            OscRouterTest test2 = new OscRouterTest();
            //test1.RunTest();
            test2.RunTest();
            Thread.Sleep(100);
            Console.WriteLine("[i] All Tests Done");
            //Main Loop
            while (true)
            {
                //Console.WriteLine("Tick");
                Thread.Sleep(100);
            }
        }
    }

   
}
