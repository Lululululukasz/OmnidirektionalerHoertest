using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerarbeitungTest
{
    internal abstract class OscSubscriber
    {
        public abstract void ReceiveOsc(string data);
    }
}
