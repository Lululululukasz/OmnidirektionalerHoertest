using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest.Tests
{
    [TestFixture]
    internal class Systemcontroller
    {
        [Test]
        public void TestConstructor()
        {
            SystemController systemController = new SystemController("127.0.0.1");
            Assert.That(systemController,Is.Not.Null);
        }

        [Test]
        public void TestTick()
        {
            SystemController systemController = new SystemController("127.0.0.1");
            Assert.That(systemController, Is.Not.Null);
            systemController.doTick();
            Assert.Pass();
        }
    }
}
