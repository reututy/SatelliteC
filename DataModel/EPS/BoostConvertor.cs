using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DataModel.EPS
{
    public class BoostConverter
    {
        public short Temperture { get; set; }
        public ushort Volt { get; set; }
        public ushort CurrentIn { get; set; }
        public ushort CurrentOut { get; set; }
        public short FixedPPTPoint { get; set; }
        //public bool IsSunny { get; set; }

        public BoostConverter(short temp, ushort vol, ushort currIn)
        {
            Temperture = temp;
            Volt = vol;
            CurrentIn = currIn;
        }

        /*public void run()
        {
            while (true)
            {
                if (IsSunny)
                {
                    CurrentIn += 10;//?????? change to constant
                    Volt += 10;//????????? change to constant
                    Temperture += 10;//???????? change to constant
                }
                Thread.Sleep(5000); //change to constant
            }
        }*/
    }
}
