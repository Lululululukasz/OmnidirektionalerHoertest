using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerarbeitungTest.Packages.Model;

namespace VerarbeitungTest
{
    internal class TestController
    {
        protected double calibrationOffset;
        protected QuestionController questionController;
        protected Action<Question> soundDomeView;
        protected Action<SoundDomeView.FeedbackType> soundDomeViewFeedback;
        protected Test test;
        protected OscRouter router;
        protected bool testStarted;
        protected long inputTimer = 0;
        protected bool callibrationRunning;
        protected double callibrationBuffer;
        protected int callibrations = 0;
        private bool readyForNextQuestion = false;
        public TestController(Action<Question> viewCallback, Action<SoundDomeView.FeedbackType> feedbackCallback, OscRouter router, double callibration)
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
            soundDomeViewFeedback = feedbackCallback;
        }
        public void startTest()
        {
            soundDomeViewFeedback(SoundDomeView.FeedbackType.start_test);
            Thread.Sleep(2000);
            testStarted = true;
            generateQuestion();
            soundDomeView(questionController.getCurrentQuestion());
        }
        private double checkAnswerOffset(double answer)
        {
            if(answer != -1)
            {
                double a = Math.Abs(answer - questionController.getCurrentQuestion().angle + calibrationOffset);
                double b = Math.Abs(answer - questionController.getCurrentQuestion().angle + calibrationOffset - 360);
                
                return a<b?a:b;
            }
            else
            {
                return answer;
            }

        }
        public void nextQuestion()
        {
            readyForNextQuestion = false;
            generateQuestion();
            soundDomeView(questionController.getCurrentQuestion());
        }
        public bool isReadyForNextQuestion() {  return readyForNextQuestion; }
        public void routerCallback(string data)
        {
            if(testStarted && (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - inputTimer > 1000)// Limit imput to once every second
            {
                inputTimer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                double answer = -1;
                data = data.Replace(',', '.');//regio code formatting
                string inputType = data.Split(':')[0];
                string answerstr = data.Split(":")[1];
                answerstr = answerstr.Remove(answerstr.Length - 1);
                if (inputType.CompareTo("alpha") == 0)
                {
                    try
                    {
                        CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        ci.NumberFormat.CurrencyDecimalSeparator = ".";
                        answer = float.Parse(answerstr, NumberStyles.Any, ci);
                        answer = answer + 90 > 360 ? (answer + 90 - 360) : (answer + 90);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Cant Parse Alpha [" + data + "]");
                    }
                }
                double offset = checkAnswerOffset(answer);
                if(offset > 40 || offset == -1)
                {
                    soundDomeViewFeedback(SoundDomeView.FeedbackType.wrong);
                    test.mistakes++;
                }
                else
                {
                    soundDomeViewFeedback(SoundDomeView.FeedbackType.correct);
                }
                test.offset.Add(offset);
                //Thread.Sleep(100);
                if(test.mistakes >= 3)
                {
                    finishTest();
                }
                else
                {
                    readyForNextQuestion = true;
                }
                
                
            }
        }
        
        
        public bool isTestFinished()
        {
            return !testStarted;
        }
        public void finishTest()
        {
            soundDomeViewFeedback(SoundDomeView.FeedbackType.test_passed);
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
