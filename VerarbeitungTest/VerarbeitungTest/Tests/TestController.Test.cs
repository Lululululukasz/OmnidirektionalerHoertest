using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace VerarbeitungTest
{
    //[TestFixture]
    internal class TestControllerTest
    {
        Question viewQuestion;
        static TestController testCtlCallibration;
        TestController testCtlTest;
        static Object _lock = new Object();
        
        //[Test]
        public void TestStartCallibration()
        {
            OscRouter router = new OscRouter();
            testCtlCallibration = new TestController(SoundDomeViewCallback, router, 0);
            Thread calThread = new Thread(new ThreadStart(SenderThreadCallibration));
            calThread.Start();
            if(testCtlCallibration.startCallibration() == 0)
            {
                Console.WriteLine("Test -> TestContoller 1 : OK");
            }
            else
            {
                Console.WriteLine("Test -> TestContoller 1 : FAILED");
            }
            //Assert.That(, Is.Zero);
            testCtlCallibration.finishTest();
            router.Close();
        }
        public void TestStartTest()
        {
            OscRouter router = new OscRouter();
            testCtlTest = new TestController(SoundDomeViewCallbackTest, router, 0);
            testCtlTest.startTest();
            double answer = testCtlTest.getQuestionController().getCurrentQuestion().angle;
            testCtlTest.routerCallback("alpha:"+answer);
            Thread.Sleep(1000);
            answer = testCtlTest.getQuestionController().getCurrentQuestion().angle;
            testCtlTest.routerCallback("alpha:" + (answer + 10));
            Thread.Sleep(1000);
            answer = testCtlTest.getQuestionController().getCurrentQuestion().angle;
            testCtlTest.routerCallback("alpha:" + (answer + 19));
            Thread.Sleep(1000);
            answer = testCtlTest.getQuestionController().getCurrentQuestion().angle;
            testCtlTest.routerCallback("alpha:" + (answer + 25));
            Thread.Sleep(1000);
            answer = testCtlTest.getQuestionController().getCurrentQuestion().angle;
            testCtlTest.routerCallback("alpha:" + (answer + 40));
            Thread.Sleep(1000);
            answer = testCtlTest.getQuestionController().getCurrentQuestion().angle;
            testCtlTest.routerCallback("alpha:" + (answer + 50));
            Thread.Sleep(1000);
            if (testCtlTest.isTestFinished())
            {
                if(testCtlTest.getTestResult().mistakes == 3 && testCtlTest.getTestResult().offset[0] == 0)
                {
                    Console.WriteLine("Test -> TestContoller 2 : OK");
                }
                else
                {
                    Console.WriteLine("Test -> TestContoller 2 : FAILED Test Result not matching");
                }
            }
            else
            {
                Console.WriteLine("Test -> TestContoller 2 : FAILED Test Didnt finished "+ testCtlTest.getTestResult().mistakes);
            }
            router.Close();
        }
        public void SoundDomeViewCallback(Question question)
        {
            viewQuestion = question;
        }
        public void SoundDomeViewCallbackTest(Question question)
        {
            
        }
        public void SenderThreadCallibration()
        {
            for(int i = 0; i < 6; i++)
            {
                Thread.Sleep(1500);
                lock (_lock)
                {
                    testCtlCallibration.routerCallibrationCallback("alpha:" + 90 * i);
                }
                
            }
        }
    }
}
