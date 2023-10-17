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
        // gernerelle lauf rutine wird einmal pro tick ausgeführt
        public void run()
        {
            nutzerEingabe();
        }

        //verabeitung und empfang der nutzerdaten ggf filter klasse hinzufügen
        void nutzerEingabe()
        {
            aktuellerModus.nutzerEingabeVerarbeitung();
        }

        //erstellen des test modus
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
