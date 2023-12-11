using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest.Tests
{
    [TestFixture]
    internal class QuestionControllerTest
    {
        QuestionController controller;
        Question q;

        [SetUp]
        public void Init()
        {
            controller = new QuestionController(dummyViewCallback);
        }

        [Test]
        public void TestRandomQuestionGeneration()
        {
            controller.generateQuestion(QuestionController.QuestionType.Random2D);

            Assert.Multiple(() => {
                Assert.That(controller.getCurrentQuestion(), Is.Not.Null);
                Assert.That(controller.getCurrentQuestion().pitch, Is.Not.Zero);
                Assert.That(controller.getCurrentQuestion().angle, Is.Not.Zero);
            });
        }

        [TestCase("input:180", 180)]
        [TestCase("input:0.5", 180)]
        [TestCase("input:0,5", 180)]
        public void TestOscQuestionGeneration(String osc, int angle)
        {
            controller.routerCallback(osc);
            controller.generateQuestion(QuestionController.QuestionType.External);

            Assert.Multiple(() => {
                Assert.That(controller.getCurrentQuestion(), Is.Not.Null);
                Assert.That(Math.Round(controller.getCurrentQuestion().angle), Is.EqualTo(angle));
                Assert.That(controller.getCurrentQuestion().pitch, Is.GreaterThan(0));
            });
        }

        [Test]
        public void TestOscQuestionGeneration()
        {
            controller.routerCallback("input:1aa8s0");
            controller.generateQuestion(QuestionController.QuestionType.External);

            Assert.Multiple(() => {
                Assert.That(controller.getCurrentQuestion(), Is.Not.Null);
                Assert.That(Math.Round(controller.getCurrentQuestion().angle), Is.EqualTo(0));
                Assert.That(controller.getCurrentQuestion().pitch, Is.Zero);
            });
        }

        [Test]
        public void TestAskQuestion()
        {
            controller.routerCallback("input:0.5");
            controller.generateQuestion(QuestionController.QuestionType.External);
            controller.askQuestion();
        }

        private void dummyViewCallback(Question q)
        {
            Assert.Multiple(() => {
                Assert.That(Math.Round(q.angle), Is.EqualTo(180));
                Assert.That(q.pitch, Is.GreaterThan(0));
            });
        }

        [Test]
        public void TestPitchGeneration()
        {
            controller.generateQuestion(QuestionController.QuestionType.Random2D);
            controller.generateQuestion(QuestionController.QuestionType.Random2D);
            controller.generateQuestion(QuestionController.QuestionType.Random2D);
            controller.generateQuestion(QuestionController.QuestionType.Random2D);
            controller.generateQuestion(QuestionController.QuestionType.Random2D);

            Assert.That(controller.getCurrentQuestion().pitch, Is.EqualTo(50));
        }
    }
}