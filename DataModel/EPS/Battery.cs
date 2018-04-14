using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    /* battery protection states */
    public enum batt_state {INITIAL, CRITICAL, SAFE, NORMAL, FULL }

    /* mode for battery[0 = normal, 1 = undervoltage, 2 = overvoltage] */
    public enum batt_mode { NORMAL, UNDERVOLTAGE, OVERVOLTAGE }

    public class Battery
    {
        public byte onboard_external { get; set; } //whether the battery is onboard or external
        public ushort vbat { get; set; }
        public ushort current_in { get; set; }
        public ushort current_out { get; set; }
        public short temperture { get; set; }
        public batt_state batt_state { get; set; }
        public batt_mode batt_mode { get; set; }

        public Battery(byte external, ushort vBat, ushort currIn, ushort currOut, short temp, batt_state state)
        {
            onboard_external = external;
            vbat = vBat;
            current_in = currIn;
            current_out = currOut;
            temperture = temp;
            batt_state = state;
        }
    }
}
