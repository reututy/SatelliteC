using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    /*MSB - [QH QS 3.3V3 3.3V2 3.3V1 5V3 5V2 5V1] - LSB*/
    /*channel types*/
    public enum channel_type { T_5V1, T_5V2, T_5V3, T_3_3V1, T_3_3V2, T_3_3V3, T_QS, T_QH }

    public class Channel
    {
        public byte status { get; set; }
        public channel_type channel_type { get; set; }
        public ushort volt { get; set; }
        public ushort latchup { get; set; }

        public Channel(byte stat, channel_type type, ushort vol, ushort latch)
        {
            status = stat;
            channel_type = type;
            volt = vol;
            latchup = latch;
        }
    }
}
