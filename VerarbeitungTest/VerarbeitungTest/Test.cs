using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    internal class Test
    {
        public int mistakes;
        public List<Question> questions;

        public Test()
        {
            this.mistakes = 0;
            this.questions = new List<Question>();
        }
    }
}
