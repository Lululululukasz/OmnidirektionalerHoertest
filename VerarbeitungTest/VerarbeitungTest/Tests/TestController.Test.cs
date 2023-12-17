using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using VerarbeitungTest.Packages.Model;

namespace VerarbeitungTest
{
    [TestFixture]
    internal class TestControllerTest
    {
        Question viewQuestion;
        static CalibrationController testCtlCallibration;
        TestController testCtlTest;
        static Object _lock = new Object();
        
        [Test]
        public void TestStartCallibration()
        {
            OscRouter router = new OscRouter();
            testCtlCallibration = new CalibrationController(SoundDomeViewCallback, SoundDomeViewCallbackFeedack, router);
            Thread calThread = new Thread(new ThreadStart(SenderThreadCallibration));
            calThread.Start();
            Assert.That(testCtlCallibration.startCallibration(), Is.Not.Null);
            testCtlCallibration.finishTest();
            router.Close();
        }
        [Test]
        public void TestStartTest()
        {
            OscRouter router = new OscRouter();
            testCtlTest = new TestController(SoundDomeViewCallbackTest, SoundDomeViewCallbackFeedack, router, 0);
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
            Assert.That(testCtlTest.isTestFinished(), Is.True);
            Assert.That(testCtlTest.getTestResult().mistakes, Is.GreaterThan(0));
            Assert.That(testCtlTest.getTestResult().offset[0], Is.Positive);
            router.Close();
        }
        public void SoundDomeViewCallback(Question question)
        {
            viewQuestion = question;
        }
        public void SoundDomeViewCallbackTest(Question question)
        {
            
        }
        public void SoundDomeViewCallbackFeedack(SoundDomeView.FeedbackType fbck)
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
