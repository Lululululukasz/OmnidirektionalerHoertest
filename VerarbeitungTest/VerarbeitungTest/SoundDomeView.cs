using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    internal class SoundDomeView
    {
        public enum FeedbackType
        {
            rise,
            fall,
            beep
        }


        private static Object _lock;
        private static Thread soundThread;
        private List<Question> QuestionQueue;
        private List<FeedbackType> FeedbackQueue;
        public void askQuestion(Question question)
        {

        }

    }
}
