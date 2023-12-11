using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    internal class TestController
    {
        protected double calibrationOffset;
        protected QuestionController questionController;
        protected Action<Question> soundDomeView;
        protected Test test;
        protected OscRouter router;
        protected bool testStarted;
        protected long inputTimer = 0;
        protected bool callibrationRunning;
        protected double callibrationBuffer;
        protected int callibrations = 0;
        public TestController(Action<Question> viewCallback, OscRouter router, double callibration)
        {
            this.router = router;
            calibrationOffset = callibration;
            router.AddReceiver(routerCallback, OscRouter.SubscriberType.Test);
            testStarted = false;
            callibrationRunning = false;
            questionController = new QuestionController(viewCallback);
            router.AddReceiver(questionController.routerCallback, OscRouter.SubscriberType.Question);
            test = new Test();
            soundDomeView = viewCallback;
        }
        public void startTest()
        {
            testStarted = true;
            generateQuestion();
        }
        private double checkAnswerOffset(double answer)
        {
            if(answer != -1)
            {
                return Math.Abs(answer-questionController.getCurrentQuestion().angle+calibrationOffset);
            }
            else
            {
                return answer;
            }

        }
        public void routerCallback(string data)
        {
            if(testStarted && (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - inputTimer > 1000)// Limit imput to once every second
            {
                inputTimer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                double answer = -1;
                data = data.Replace(',', '.');//regio code formatting
                string inputType = data.Split(':')[0];
                string answerstr = data.Split(":")[1];
                if (inputType.CompareTo("alpha") == 0)
                {
                    try
                    {
                        answer = double.Parse(answerstr);
                    }
                    catch (Exception e)
                    {

                    }
                }
                double offset = checkAnswerOffset(answer);
                if(offset > 20 || offset == -1)
                {
                    test.mistakes++;
                }
                test.offset.Add(offset);
                if(test.mistakes >= 3)
                {
                    finishTest();
                }
                else
                {
                    Question question = new Question();
                    question.angle = answer;
                    question.pitch = answer == -1 ? 50 : 1200;
                    soundDomeView(question);
                    Thread.Sleep(500);
                    generateQuestion();
                }
                
                
            }
        }
        
        
        public bool isTestFinished()
        {
            return !testStarted;
        }
        public void finishTest()
        {
            GuiController.SendResultToGui(test);
            testStarted = false;
        }
        public Test getTestResult()
        {
            return test;
        }
        private void generateQuestion()
        {
            questionController.generateQuestion(QuestionController.QuestionType.Random2D);
        }
        public QuestionController getQuestionController()
        {
            return questionController;
        }

    }
}
