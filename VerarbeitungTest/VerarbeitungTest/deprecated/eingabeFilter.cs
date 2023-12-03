using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rug.Osc;

namespace VerarbeitungTest
{
    internal class EingabeFilter
    {
       
        public EingabeFilter() { }  

        public OscPacket filter(OscPacket oscPacket)
        {
            return oscPacket;
        }
    }
}