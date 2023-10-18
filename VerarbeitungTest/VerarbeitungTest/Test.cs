using Rug.Osc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
        private SoundDomeSender sender;

        public Test() {
            Console.WriteLine("Neuen Test Eratellt!");
            zielPunkt = new ZielPunkt(0);
            modusHinzufügen();
            OSCReceiver = new OSCReceiver(this);
            eingabeFilter = new EingabeFilter();
            sender = new SoundDomeSender("127.0.0.1",9000);

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
                foreach (OscPacket item in packs)
                {
                    string pack = item.ToString().Split(' ')[0];
                    string _number = item.ToString().Split(' ')[1].Split('f')[0];
                    float number = float.Parse(_number, CultureInfo.InvariantCulture.NumberFormat);
                    Console.WriteLine("Adresse: "+pack+" Wert:"+number);
                    switch (pack) 
                    {
                        case "/I1-2/mouse/deg":
                            sender.sendeDeg(2, number, 90, 0.5f);
                            break;
                        case "/I1-2/mouse/degSchieben":
                            sender.sendeDeg(3, 90 , number, 0.5f);
                            break;
                        case "/I1-2/mouse/rawx":

                            break;
                        case "/I1-2/mouse/rawy":

                            break;
                    }
                }
                Console.WriteLine("Gesammelte Packete-------------");
                nutzerEingabe();//OSC Daten An logik weiterleiten
            }
        }
    }
}
