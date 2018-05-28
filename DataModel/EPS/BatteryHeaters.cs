using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    public class BatteryHeater
    {
        public byte Mode { get; set; } //0 = Manual, 1 = Auto]
        public byte Type { get; set; } //0 = BP4, 1= Onboard
        public byte Status { get; set; } //0 = OFF 1 = ON
        public sbyte BattHeaterLow { get; set; }   //! Turn heater on at [degC]
        public sbyte BattHeaterHigh { get; set; }  //! Turn heater off at [degC]

        public BatteryHeater(byte mod, byte typ, byte stat, sbyte low, sbyte high)
        {
            Mode = mod;
            Type = typ;
            Status = stat;
            BattHeaterLow = low;
            BattHeaterHigh = high;
        }
    }
}
