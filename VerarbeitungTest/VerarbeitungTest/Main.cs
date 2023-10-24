using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    class Main
    {
        public void run()
        {
            Test t = new Test();

            //Testweise Haupt rutine genauere steuerung der tests hier ggf einfügen
            while (true)
            {
                t.run();
                Thread.Sleep(25);
            }
        }
    }

   
}
