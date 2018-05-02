using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    /* battery protection states */
    public enum batt_state {INITIAL, CRITICAL, SAFE, NORMAL, FULL }

    /* mode for battery[0 = normal, 1 = undervoltage, 2 = overvoltage] */
    public enum batt_mode { NORMAL, UNDERVOLTAGE, OVERVOLTAGE }

    public class Battery : INotifyPropertyChanged
    {
        public byte onboard_external { get; set; } //whether the battery is onboard or external
       
        public event PropertyChangedEventHandler PropertyChanged;

        private ushort _vbat;

        public ushort Vbat {
            get
            {
                return _vbat;
            }
            set
            {
                _vbat = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Vbat"));
                }
            }
        }
        public ushort current_in { get; set; }
        public ushort current_out { get; set; }
        public short temperture { get; set; }
        public batt_state batt_state { get; set; }
        public batt_mode batt_mode { get; set; }

        public Battery(byte external, ushort vBat, ushort currIn, ushort currOut, short temp, batt_state state)
        {
            onboard_external = external;
            Vbat = vBat;
            current_in = currIn;
            current_out = currOut;
            temperture = temp;
            batt_state = state;
        }


        public void Run()
        {
            while (true)
            {

            }
        }


 

       /* private void CheckBatteryState()
        {
            switch (batt_state)
            {
                case batt_state.INITIAL:
                    if (Vbat < EPSConstants.CRITICAL_VBAT)
                        batt_state = batt_state.CRITICAL;
                    else if (Vbat < EPSConstants.SAFE_VBAT)
                        batt_state = batt_state.SAFE;
                    else if (Vbat < EPSConstants.MAX_VBAT)
                        batt_state = batt_state.NORMAL;
                    else
                    {
                        batt_state = batt_state.FULL;
                        HardwareHighVoltProtection();
                    }
                    break;
                case batt_state.CRITICAL:
                    break;
                case batt_state.SAFE:
                    break;
                case batt_state.NORMAL:
                    break;
                case batt_state.FULL:
                    break;
            }
        }

        public void BatteryDrop()
        {
            Vbat -= 10; //need to be changed
            current_out -= 10; //need to be changed
            CheckBatteryState();
        }*/
    }
}
