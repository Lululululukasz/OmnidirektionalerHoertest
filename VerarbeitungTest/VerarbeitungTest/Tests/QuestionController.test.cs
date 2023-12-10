using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest.Tests
{
    /// <summary>
    /// Test Class for the QuestionController, to start the Test use [QuestionControllerTest.RunTest()]
    /// </summary>
    internal class QuestionControllerTest
    {
        private int successfulTests;
        private int requieredTests;

        public QuestionControllerTest()
        {
            successfulTests = 0;
            requieredTests = 1;
        }
        /// <summary>
        /// Run the Test
        /// </summary>
        public void RunTest()
        {

            QuestionController controller = new QuestionController(dummyViewCallback);
            controller.generateQuestion(QuestionController.QuestionType.Random2D);
            Question q = controller.getCurrentQuestion();
            if (q != null && q.pitch != 0 && q.angle != 0)
            {//Test 1 check if controller returns not null and not default object
                successfulTests++;
            }
            else
            {
                Console.WriteLine("[i] Subtest " + requieredTests + " not Passed!");
            }

            requieredTests++;
            controller.routerCallback("input:0.5");//!!!!!REGIONAL FORMAT FIXEN!!!!!!
            controller.routerCallback("input:0,5");
            controller.routerCallback("input:180");
            controller.generateQuestion(QuestionController.QuestionType.External);
            q = controller.getCurrentQuestion();
            if (Math.Round(q.angle) == 180 && q.pitch > 0)
            {
                successfulTests++;
            }
            else
            {
                Console.WriteLine("[i] Subtest " + requieredTests + " not Passed!");
            }

            requieredTests++;
            controller = new QuestionController(dummyViewCallback);
            controller.routerCallback("input:0.5");
            controller.generateQuestion(QuestionController.QuestionType.External);
            q = controller.getCurrentQuestion();
            if (Math.Round(q.angle) == 180 && q.pitch > 0)
            {
                successfulTests++;
            }
            else
            {
                Console.WriteLine("[i] Subtest " + requieredTests + " not Passed!");
            }

            requieredTests++;
            controller = new QuestionController(dummyViewCallback);
            controller.routerCallback("input:0,5");
            controller.generateQuestion(QuestionController.QuestionType.External);
            q = controller.getCurrentQuestion();
            if (Math.Round(q.angle) == 180 && q.pitch > 0)
            {
                successfulTests++;
            }
            else
            {
                Console.WriteLine("[i] Subtest " + requieredTests + " not Passed!");
            }

            requieredTests++;
            controller = new QuestionController(dummyViewCallback);
            controller.routerCallback("input:180");
            controller.generateQuestion(QuestionController.QuestionType.External);
            q = controller.getCurrentQuestion();
            if (Math.Round(q.angle) == 180 && q.pitch > 0)
            {
                successfulTests++;
            }
            else
            {
                Console.WriteLine("[i] Subtest " + requieredTests + " not Passed!");
            }

            requieredTests++;
            controller = new QuestionController(dummyViewCallback);
            controller.routerCallback("input:1aa8s0");
            controller.generateQuestion(QuestionController.QuestionType.External);
            q = controller.getCurrentQuestion();
            if (Math.Round(q.angle) == 0 && q.pitch == 0)
            {
                successfulTests++;
            }
            else
            {
                Console.WriteLine("[i] Subtest " + requieredTests + " not Passed!");
            }

            requieredTests++;
            controller = new QuestionController(dummyViewCallback);
            controller.routerCallback("input:0.5");
            controller.generateQuestion(QuestionController.QuestionType.External);
            controller.askQuestion();

            requieredTests++;
            controller = new QuestionController(dummyViewCallback);
            controller.generateQuestion(QuestionController.QuestionType.Random2D);
            controller.generateQuestion(QuestionController.QuestionType.Random2D);
            controller.generateQuestion(QuestionController.QuestionType.Random2D);
            controller.generateQuestion(QuestionController.QuestionType.Random2D);
            controller.generateQuestion(QuestionController.QuestionType.Random2D);
            q = controller.getCurrentQuestion();
            if (q.pitch == 50)
            {
                successfulTests++;
            }
            else
            {
                Console.WriteLine("[i] Subtest " + requieredTests + " not Passed!");
            }

            //Check Subtests
            if (successfulTests == requieredTests)
            {
                Console.WriteLine("Test -> QuestionController : OK " + successfulTests + "/" + requieredTests);
            }
            else
            {
                Console.WriteLine("Test -> QuestionController : FAILED " + successfulTests + "/" + requieredTests);
            }
        }
        private void dummyViewCallback(Question q)
        {
            if (Math.Round(q.angle) == 180 && q.pitch > 0)
            {
                successfulTests++;
            }
            else
            {
                Console.WriteLine("[i] Subtest " + requieredTests + " not Passed!");
            }
        }
    }
}
