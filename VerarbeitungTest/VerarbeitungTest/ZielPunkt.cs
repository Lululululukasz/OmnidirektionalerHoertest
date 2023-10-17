using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VerarbeitungTest
{
    internal class ZielPunkt
    {
        public Types.Vector3 position { get; }

        public ZielPunkt(int param) {
            if(param == 0)
            {
                position = GeneriereZuf�llig();
            }
        }



        Types.Vector3 GeneriereZuf�llig()
        {
            Random r = new Random();
            return new Types.Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
        }
    }
}
