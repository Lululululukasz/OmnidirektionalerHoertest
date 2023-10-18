using Rug.Osc;
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
        private OSCReceiver OSCReceiver;
        private EingabeFilter eingabeFilter;

        public Test() {
            Console.WriteLine("Neuen Test Eratellt!");
            zielPunkt = new ZielPunkt(0);
            modusHinzufügen();
            OSCReceiver = new OSCReceiver(this);
            eingabeFilter = new EingabeFilter();

        }
        // gernerelle lauf rutine wird einmal pro tick ausgeführt
        public void run()
        {
            verabeiteOSC();
            
            
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
        void verabeiteOSC()
        {
            if ( OSCReceiver != null )
            {
                List<OscPacket> packs = new List<OscPacket>(OSCReceiver.Packages);
                if (packs.Count == 0) return;
                OSCReceiver.ClearBuffer();
                Console.WriteLine("Gesammelte Packete+++++++++++++");
                foreach (var item in packs)
                {
                    Console.WriteLine(item.ToString());
                }
                Console.WriteLine("Gesammelte Packete-------------");
                nutzerEingabe();//OSC Daten An logik weiterleiten
            }
        }
    }
}
