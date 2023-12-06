using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace VerarbeitungTest
{
    [TestFixture]
    internal class TestTest
    {
        private const int expectedInitMistakes = 0;
        private List<Question> expectedInitQuestions = new List<Question>();
        private Test test = new Test();


        [Test]
        public void testInitMistakes()
        {
            Assert.That(test.mistakes, Is.Zero);
        }

        [Test]
        public void testInitQuestions()
        {
            Assert.That(test.offset, Is.Empty);
        }
    }
}
