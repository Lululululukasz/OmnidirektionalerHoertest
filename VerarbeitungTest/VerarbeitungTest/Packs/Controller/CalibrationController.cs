using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerarbeitungTest.Packages.Model;

namespace VerarbeitungTest
{
    internal class CalibrationController : TestController
    {
        public CalibrationController(Action<Question> viewCallback, Action<SoundDomeView.FeedbackType> feedbackCallback, OscRouter router): base(viewCallback, feedbackCallback, router,0)
        {
            
        }

        public double startCallibration()
        {
            soundDomeViewFeedback(SoundDomeView.FeedbackType.start_callibration);
            Thread.Sleep(2000);
            double cal = 0;
            router.AddReceiver(routerCallibrationCallback, OscRouter.SubscriberType.Test);
            callibrationBuffer = 0;

            Question q = new Question();
            q.angle = 0;
            q.pitch = 200;
            questionController.generateQuestion(q);
            questionController.askQuestion();
            callibrationRunning = true;
            long timeoutTimer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            while (callibrationRunning && (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - timeoutTimer < 30000)//timeout for callibration if no inputs happen
            {
                Thread.Sleep(500);
            }
            cal = callibrationBuffer / 4;
            callibrationRunning = false;
            soundDomeViewFeedback(SoundDomeView.FeedbackType.callibration_passed);
            return cal;
        }

        public void routerCallibrationCallback(string data)
        {
            if (callibrationRunning && (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - inputTimer > 1000)// Limit imput to once every second
            {
                inputTimer = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                double answer = -1;
                data = data.Replace(',', '.');//regio code formatting
                string inputType = data.Split(':')[0];
                string answerstr = data.Split(":")[1];
                answerstr = answerstr.Remove(answerstr.Length - 1);
                //Console.WriteLine("Raw Answer:" + answerstr);
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
                        Console.WriteLine("Cant Parse Answerstring!");
                    }
                }
                double offset = Math.Abs(answer - questionController.getCurrentQuestion().angle);
                Console.WriteLine("Offset is " + offset + " Answ: "+answer+" Quest: "+ questionController.getCurrentQuestion().angle);
                callibrationBuffer += offset;
                callibrations++;
                if (callibrations > 3)
                {
                    callibrationRunning = false;
                }
                else
                {
                    Question q = questionController.getCurrentQuestion();
                    q.angle += 90;
                    questionController.generateQuestion(q);
                    questionController.askQuestion();
                    //Thread.Sleep(1200);
                }

            }
        }

    }
}
