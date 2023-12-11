using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    [TestFixture]
    internal class OscRouterAddRecieverTest
    {

        OscRouter router = new OscRouter();

        String message = "";

        private void Callback(string data)
        {
            message = data;
        }


        [Test]
        public void TestSystemReciever()
        {
            router.AddReceiver(Callback, OscRouter.SubscriberType.System);
            router.Route("/I1-2/mouse/click, 1");
            Assert.That(message, Is.EqualTo("click:1"));
        }

        [Test]
        public void TestTestReciever()
        {
            router.AddReceiver(Callback, OscRouter.SubscriberType.Test);
            router.Route("/I1-2/mouse/alpha, 180");
            Assert.That(message, Is.EqualTo("alpha:180"));
        }

        [Test]
        public void TestQuestionReciever()
        {
            router.AddReceiver(Callback, OscRouter.SubscriberType.Question);
            router.Route("/Ix-x/input/sampleInput, 42");
            Assert.That(message, Is.EqualTo("sampleInput:42"));
        }
    }
}