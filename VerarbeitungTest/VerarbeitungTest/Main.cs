using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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
            //OscSender sender = new OscSender(IPAddress.Parse("127.0.0.1"), 9001);
            //sender.Connect();
            //SoundDomeSender sds = new SoundDomeSender("127.0.0.1",9001);
            SystemController controller = new SystemController();

            //Main Loop

            while (true)
            {
                //sds.sendeDeg(1, 60, 0, 0.8f);
                //sender.Send(new OscMessage("/adm/obj/1/azim", (float)40f));
                //sender.Send(new OscMessage("/adm/obj/1/elev", (float)0f));
                //sender.Send(new OscMessage("/adm/obj/1/dist", (float)0.8f));
                //Console.WriteLine("Tick");
                Thread.Sleep(10);
            }
        }
    }

   
}
