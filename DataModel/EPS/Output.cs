using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    /*channel types*/
    public enum OutputType { T_5V1, T_5V2, T_5V3, T_3_3V1, T_3_3V2, T_3_3V3, T_QS, T_QH }

    public class Output
    {
        public byte Status { get; set; }
        public OutputType ChannelType { get; set; }
        public ushort Volt { get; set; }

        public Output(byte stat, OutputType type, ushort vol)
        {
            Status = stat;
            ChannelType = type;
            Volt = vol;
        }
    }
}
