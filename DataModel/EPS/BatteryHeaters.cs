using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    public class BatteryHeaters
    {
        public byte mode { get; set; } //0 = Manual, 1 = Auto]
        public byte type { get; set; } //0 = BP4, 1= Onboard
        public byte status { get; set; } //0 = OFF 1 = ON
        public sbyte battheater_low { get; set; }   //! Turn heater on at [degC]
        public sbyte battheater_high { get; set; }  //! Turn heater off at [degC]

        public BatteryHeaters(byte mod, byte typ, byte stat, sbyte low, sbyte high)
        {
            mode = mod;
            type = typ;
            status = stat;
            battheater_low = low;
            battheater_high = high;
        }
    }
}
