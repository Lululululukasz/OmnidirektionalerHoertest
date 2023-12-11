using System;
using System.Collections.Generic;
using System.Linq;
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
            TestControllerTest testController = new TestControllerTest();
            testController.TestStartCallibration();
            testController.TestStartTest();


            SoundDomeView sdv = new SoundDomeView();
            Question q = new Question();
            q.angle = 0;
            q.pitch = 400;

            sdv.askQuestion(q);
            sdv.giveFeedback(SoundDomeView.FeedbackType.rise);
            sdv.giveFeedback(SoundDomeView.FeedbackType.fall);
            sdv.giveFeedback(SoundDomeView.FeedbackType.beep);

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
