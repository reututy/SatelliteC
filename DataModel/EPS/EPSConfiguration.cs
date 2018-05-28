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
        public byte battheater_mode { get; set; } //!< Mode for battheater [0 = Manual, 1 = Auto]
        public byte battheater_low { get; set; } //!< Turn heater on at [degC]
        public byte battheater_high { get; set; } //!< Turn heater off at [degC]
        public byte[] output_normal_value { get; set; } //!< Nominal mode output value
        public byte[] output_safe_value { get; set; } //!< Safe mode output value
        public ushort[] output_initial_on_delay { get; set; } //!< Output switches: init with these on delays [s]
        public ushort[] output_initial_off_delay { get; set; }//!< Output switches: init with these off delays [s]
        public ushort[] vboost { get; set; } //!< Fixed PPT point for boost converters [mV]
        //public ushort commandReply { get; set; } //!< reply of the last command
        public ushort batt_maxvoltage { get; set; } //!< Maximum battery voltage
        public ushort batt_safevoltage { get; set; } //!< Battery voltage for safe mode
        public ushort batt_criticalvoltage { get; set; } //!< Battery voltage for critical mode
        public ushort batt_normalvoltage { get; set; } //!< Battery voltage for normal mode

        public EPSConfiguration(byte mode, byte heatMode, byte heatLow, byte heatHigh, byte[] outNormal, byte[] outSafe,
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

        public void AddConfiguration2(ushort max, ushort safe, ushort critical, ushort normal)
        {
            batt_maxvoltage = max;
            batt_safevoltage = safe;
            batt_criticalvoltage = critical;
            batt_normalvoltage = normal;
        }
    }
}
