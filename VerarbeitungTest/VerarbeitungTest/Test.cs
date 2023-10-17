using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    internal class Test
    {

        private ZielPunkt zielPunkt;
        private Modus aktuellerModus;


        public Test() {
            Console.WriteLine("Neuen Test Eratellt!");
            zielPunkt = new ZielPunkt(0);
            modusHinzufügen();

        }

        public void run()
        {
            nutzerEingabe();
        }

        void nutzerEingabe()
        {
            aktuellerModus.nutzerEingabeVerarbeitung();
        }
        void modusHinzufügen()
        {
            aktuellerModus = new _2DTest();
        }
        void kalibrieren()
        {
            //WIP
        }
    }
}
