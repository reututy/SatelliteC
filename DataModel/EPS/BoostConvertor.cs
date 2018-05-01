using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DataModel.EPS
{
    public class BoostConvertor
    {
        public short temperture { get; set; }
        public ushort volt { get; set; }
        public ushort current_in { get; set; }
        public bool IsSunny { get; set; }

        public BoostConvertor(short temp, ushort vol, ushort currIn)
        {
            temperture = temp;
            volt = vol;
            current_in = currIn;
        }

        public void run()
        {
            while (true)
            {
                if (IsSunny)
                {
                    current_in += 10;//?????? change to constant
                    volt += 10;//????????? change to constant
                    temperture += 10;//???????? change to constant
                }
                Thread.Sleep(5000); //change to constant
            }
        }
    }
}
