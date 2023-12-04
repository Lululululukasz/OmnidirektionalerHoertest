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

            QuestionController controller = new QuestionController();
            controller.GenerateQuestion(QuestionController.QuestionType.Random2D);
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
            controller.RouterCallback("input:0.5");//!!!!!REGIONAL FORMAT FIXEN!!!!!!
            controller.RouterCallback("input:0,5");
            controller.RouterCallback("input:180");
            controller.GenerateQuestion(QuestionController.QuestionType.External);
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
            controller = new QuestionController();
            controller.RouterCallback("input:0.5");
            controller.GenerateQuestion(QuestionController.QuestionType.External);
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
            controller = new QuestionController();
            controller.RouterCallback("input:0,5");
            controller.GenerateQuestion(QuestionController.QuestionType.External);
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
            controller = new QuestionController();
            controller.RouterCallback("input:180");
            controller.GenerateQuestion(QuestionController.QuestionType.External);
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
            controller = new QuestionController();
            controller.RouterCallback("input:1aa8s0");
            controller.GenerateQuestion(QuestionController.QuestionType.External);
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
            controller.RouterCallback("input:0.5");
            controller.GenerateQuestion(QuestionController.QuestionType.External);
            controller.askQuestion();




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
