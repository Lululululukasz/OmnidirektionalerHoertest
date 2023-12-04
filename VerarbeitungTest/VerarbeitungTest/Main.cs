using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerarbeitungTest.Tests;

namespace VerarbeitungTest
{
    class Main
    {
        public void run()
        {
            
            QuestionControllerTest questionControllertest = new QuestionControllerTest();
            questionControllertest.RunTest();
            
            

            //Testweise Haupt rutine genauere steuerung der tests hier ggf einfügen
            while (true)
            {
                Thread.Sleep(25);
            }
        }
    }

   
}
