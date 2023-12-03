using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    internal class OscRouterTest
    {
        int passedSubTests = 0;
        private void SystemCallback(string data)
        {
            //Console.WriteLine(data);
            if (data.CompareTo("click:1") == 0) { passedSubTests++; }
        }
        private void TestCallback(string data)
        {
            //Console.WriteLine(data);
            if (data.CompareTo("alpha:180") == 0) { passedSubTests++; }
        }
        private void QuestionCallback(string data)
        {
            //Console.WriteLine(data);
            if (data.CompareTo("sampleInput:42") == 0) { passedSubTests++; }
        }
        public void RunTest()
        {
            OscReceiver dummy = null;
            OscRouter router = new OscRouter(dummy);
            router.AddReceiver(SystemCallback, OscRouter.SubscriberType.System);
            router.AddReceiver(TestCallback, OscRouter.SubscriberType.Test);
            router.AddReceiver(QuestionCallback, OscRouter.SubscriberType.Question);
            
            router.Route("/I1-2/mouse/click, 1");
            router.Route("/I1-2/mouse/alpha, 180");
            router.Route("/Ix-x/input/sampleInput, 42");
            if(passedSubTests == 3)
            {
                Console.WriteLine("Test -> OscRouter : OK");
            }
            else
            {
                Console.WriteLine("Test -> OscRouter : FAILED "+passedSubTests+"/3");
            }
        }

    }
}
