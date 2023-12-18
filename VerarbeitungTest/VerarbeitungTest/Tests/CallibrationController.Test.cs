using NUnit.Framework;
using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerarbeitungTest.Packages.Model;

namespace VerarbeitungTest.Tests
{
    [TestFixture]
    internal class CallibrationControllerTest
    {
        CalibrationController controller;
        [Test]
        public void TestCallibration()
        {
            OscRouter router = new OscRouter();
            controller = new CalibrationController(DummyView, DummyViewFeedback,router);
            Thread t = new Thread(new ThreadStart(SendThread));
            t.Start();
            double d = controller.startCallibration();
            router.Close();
            Assert.That(d,Is.Not.Zero);
        }

        public void DummyView(Question q)
        {

        }
        public void DummyViewFeedback(SoundDomeView.FeedbackType f) { }

        public void SendThread()
        {
            Thread.Sleep(1000);
            controller.routerCallibrationCallback("alpha:60");
            Thread.Sleep(2000);
            controller.routerCallibrationCallback("alpha:60");
            Thread.Sleep(2000);
            controller.routerCallibrationCallback("alpha:60");
            Thread.Sleep(2000);
            controller.routerCallibrationCallback("alpha:60");
            Thread.Sleep(2000);
        }
    }
}
