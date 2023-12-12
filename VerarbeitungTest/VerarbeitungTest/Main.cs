using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VerarbeitungTest.Tests;

namespace VerarbeitungTest
{
    class Main
    {
        public static void run()
        {

            //Run Unit Tests    
            //TestControllerTest testController = new TestControllerTest();
            //testController.TestStartCallibration();
            //testController.TestStartTest();

            SystemController controller = new SystemController();
            
            //Main Loop

            while (true)
            {
                //Console.WriteLine("Tick");
                Thread.Sleep(100);
            }
        }
    }

   
}
