using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    public class BoostConvertor
    {
        public short temperture { get; set; }
        public ushort volt { get; set; }
        public ushort current_in { get; set; }

        public BoostConvertor(short temp, ushort vol, ushort currIn)
        {
            temperture = temp;
            volt = vol;
            current_in = currIn;
        }
    }
}
