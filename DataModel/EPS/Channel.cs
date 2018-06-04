using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    /*MSB - [QH QS 3.3V3 3.3V2 3.3V1 5V3 5V2 5V1] - LSB*/
    

    public class Channel : Output
    {
        
        public ushort CurrentOut { get; set; }
        public ushort LatchupNum { get; set; }

        public Channel(byte stat, OutputType type, ushort vol, ushort currOut, ushort latch) : base(stat, type, vol)
        {
            CurrentOut = currOut;
            LatchupNum = latch;
        }
    }
}
