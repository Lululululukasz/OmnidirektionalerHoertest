using NUnit.Framework;
using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest.Tests
{
    [TestFixture]
    internal class SoundDomeViewTest
    {
        private Thread netThread;
        private bool killNetThread = false;
        private string netData = "";
        private Rug.Osc.OscReceiver receiver;
        private static Object _lock;

        [Test]
        public void testMp3Loading()
        {
            SoundDomeView soundDomeView = new SoundDomeView("127.0.0.1");
            Assert.That(soundDomeView.getMp3Dictionary(),Is.Not.Null);
            Assert.That(soundDomeView.getMp3Dictionary()[SoundDomeView.FeedbackType.start_test], Is.Not.Null);
            Assert.That(soundDomeView.getMp3Dictionary()[SoundDomeView.FeedbackType.start_test].Length, Is.AtLeast(1024));
        }
        [Test]
        public void testAskQuestion()
        {
            SoundDomeView soundDomeView = new SoundDomeView("127.0.0.1");
            _lock = new Object();
            netThread = new Thread(new ThreadStart(DummyNetThread));
            receiver = new Rug.Osc.OscReceiver(9000);
            receiver.Connect();
            netThread.Start();
            soundDomeView.askQuestion(new Packages.Model.Question { angle=180,pitch=500});
            
            Assert.Pass();
           

        }

        [Test]
        public void testFeedback()
        {
            SoundDomeView soundDomeView = new SoundDomeView("127.0.0.1");
            Thread.Sleep(2000);
            soundDomeView.giveFeedback(SoundDomeView.FeedbackType.start_test);
            Thread.Sleep(2000);
            soundDomeView.giveFeedback(SoundDomeView.FeedbackType.start_callibration);
            Thread.Sleep(2000);
            soundDomeView.giveFeedback(SoundDomeView.FeedbackType.callibration_passed);
            Thread.Sleep(2000);
            soundDomeView.giveFeedback(SoundDomeView.FeedbackType.test_passed);
            Thread.Sleep(2000);
            soundDomeView.giveFeedback(SoundDomeView.FeedbackType.test_not_passed);
            Thread.Sleep(3000);
            Assert.Pass();
        }

        public void DummyNetThread()
        {
            while (receiver.State != OscSocketState.Closed)
            {

                if (receiver.State == OscSocketState.Connected)
                {

                    OscPacket packet = receiver.Receive();
                    //ThreadSafe packet zur liste hinzufügen
                    lock (_lock)
                    {
                        netData = packet.ToString();
                    }

                }
            }
        }
    }
}
