using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerarbeitungTest.Packages.Model;

namespace VerarbeitungTest
{
    /// <summary>
    /// QuestionController Class:
    /// <para>This Class is for generating Questions from a Randomgenerator or</para>
    /// <para>from External Data.</para>
    /// <para>Its also resposible for Sending the Question data to the SoundDomeView</para>
    /// </summary>
    internal class QuestionController
    {
        private Question question;
        private List<double> otherGroupData;
        private Random rng;
        Action<Question> viewCallback;
        private int currentTestTone;
        /// <summary>
        /// Constructor for Standart SoundDomeView
        /// </summary>
        public QuestionController(SoundDomeView view)
        {
            otherGroupData = new List<double>();
            rng = new Random();
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            currentTestTone = 100;
            viewCallback = view.askQuestion;
        }
        /// <summary>
        /// Constructor for Custom View Callback
        /// </summary>
        public QuestionController(Action<Question> callback)
        {
            otherGroupData = new List<double>();
            rng = new Random();
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            currentTestTone = 100;
            viewCallback = callback;
        }
        public enum QuestionType
        {
            External,
            Random2D
        }
        /// <summary>
        /// Asks the currently active Question (Sends data to SoundDomeView)
        /// </summary>
        public void askQuestion()
        {
            if(viewCallback != null)
            {
                viewCallback(question);
            }
            else
            {
                Console.WriteLine("No View Available Printing Question into Console:\n angle=" + question.angle + " pitch=" + question.pitch);
            }
        }
        /// <summary>
        /// Generates a Question and saves it
        /// <para>type -> QuestionType:Enum Selects the source of the Questiondata</para>
        /// </summary>
        public void generateQuestion(QuestionType type)
        {
            switch (type)
            {
                case QuestionType.External:
                    {
                        if(otherGroupData.Count == 0)
                        {
                            question = new Question();
                            break;
                        }
                        double targetDirection = 0;
                        //choose Random Data Entry
                        int index = rng.Next(otherGroupData.Count);
                        double choosenOne = otherGroupData[index];

                        //Differetiate between External data Ranges
                        if (choosenOne < 1)
                        {
                            targetDirection = choosenOne * 360;
                        }
                        else
                        {
                            targetDirection = choosenOne;
                        }
                        Question q = new Question();
                        //generate test Tone
                        if (currentTestTone < 100)
                        {
                            currentTestTone += 10;
                        }
                        else if (currentTestTone < 1000)
                        {
                            currentTestTone += 100;
                        }
                        else if (currentTestTone >= 1000)
                        {
                            currentTestTone += 200;
                        }
                        if (currentTestTone >= 3000)
                        {
                            currentTestTone = 100;
                        }
                        q.pitch = currentTestTone;
                        q.angle = targetDirection;
                        question = q;
                    }
                    break;
                case QuestionType.Random2D:
                    {
                        Question q = new Question();
                        //generate test Tone
                        if (currentTestTone < 100)
                        {
                            currentTestTone += 10;
                        }
                        else if (currentTestTone < 1000)
                        {
                            currentTestTone += 100;
                        }
                        else if (currentTestTone >= 1000)
                        {
                            currentTestTone += 200;
                        }
                        if(currentTestTone >= 3000)
                        {
                            currentTestTone = 100;
                        }
                        q.pitch = currentTestTone;
                        q.angle = rng.NextDouble() * 359 + 1;
                        question = q;
                    }
                    break;
            }
        }
        public void generateQuestion(Question q)
        {
            question = q;
        }
        public Question getCurrentQuestion()
        {
            if(question == null)
            {
                return new Question();
            }
            else
            {
                return question;
            } 

        }
        /// <summary>
        /// Checks if any External data is in the Buffer
        /// </summary>
        public bool externalDataAvailable()
        {
            if(otherGroupData.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void routerCallback(string data)
        {
            data = data.Replace(',', '.');//regio code formatting
            try
            {
                otherGroupData.Add(double.Parse(data.Split(':')[1]));
            }catch(Exception e)
            {

            }
            
        }
        public void resetQuestionController()
        {
            otherGroupData = new List<double>();
            rng = new Random();
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            currentTestTone = 10;
        }
    }
}
