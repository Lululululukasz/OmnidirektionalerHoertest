using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    internal class GuiController
    {
        public static void SendResultToGui(Test test)
        {
            
            OscSender sender = new OscSender(IPAddress.Parse("127.0.0.1"), 10000);
            sender.Connect();
            sender.Send(new OscMessage("/I1-2/mouse/Test_Mistakes", test.mistakes));
            Thread.Sleep(10);
            int i = 1;
            foreach(double d in test.offset)
            {
                sender.Send(new OscMessage("/I1-2/mouse/TestResult"+i, d));
                i++;
                Thread.Sleep(10);
            }
            sender.Close();
        }
    }
}
