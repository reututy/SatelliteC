using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    public class EPSConfiguration
    {
        public byte ppt_mode { get; set; } //!< Mode for PPT [1 = AUTO, 2 = FIXED]
        public byte battheater_mode; //!< Mode for battheater [0 = Manual, 1 = Auto]
        public char battheater_low; //!< Turn heater on at [degC]
        public char battheater_high; //!< Turn heater off at [degC]
        public byte[] output_normal_value; //!< Nominal mode output value
        public byte[] output_safe_value; //!< Safe mode output value
        public ushort[] output_initial_on_delay; //!< Output switches: init with these on delays [s]
        public ushort[] output_initial_off_delay;//!< Output switches: init with these off delays [s]
        public ushort[] vboost; //!< Fixed PPT point for boost converters [mV]

        public EPSConfiguration(byte mode, byte heatMode, char heatLow, char heatHigh, byte[] outNormal, byte[] outSafe,
            ushort[] outOnDel, ushort[] outOffDel, ushort[] vBoost)
        {
            ppt_mode = mode;
            battheater_mode = heatMode;
            battheater_low = heatLow;
            battheater_high = heatHigh;
            output_normal_value = outNormal;
            output_safe_value = outSafe;
            output_initial_on_delay = outOnDel;
            output_initial_off_delay = outOffDel;
            vboost = vBoost;
        }
    }
}
