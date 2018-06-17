using System.ComponentModel;

namespace DataModel.EPS
{
    /* battery protection states */
    public enum BattState {INITIAL, CRITICAL, SAFE, NORMAL, FULL}

    /* mode for battery[0 = normal, 1 = undervoltage, 2 = overvoltage] */
    //public enum BattMode { NORMAL, UNDERVOLTAGE, OVERVOLTAGE }

    public class Battery : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public byte OnboardExternal { get; set; } //whether the battery is onboard or external

        private ushort _vbat;
        public ushort Vbat {
            get
            {
                return _vbat;
            }
            set
            {
                if (value >= EPSConstants.BAT_CONNECT_V_MIN && value <= EPSConstants.BAT_CONNECT_V_MAX)
                {
                    _vbat = value;
                }
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Vbat"));
                }
            }
        }

        private ushort _currentIn;
        public ushort CurrentIn
        {
            get
            {
                return _currentIn;
            }
            set
            {
                _currentIn = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentIn"));
                }
            }
        }

        private ushort _currentOut;
        public ushort CurrentOut
        {
            get
            {
                return _currentOut;
            }
            set
            {
                _currentOut = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentOut"));
                }
            }
        }

        private short _temperture;
        public short Temperture
        {
            get
            {
                return _temperture;
            }
            set
            {
                _temperture = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Temperture"));
                }
            }
        }

        private BattState _battState;
        public BattState BattState
        {
            get
            {
                return _battState;
            }
            set
            {
                _battState = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BattState"));
                }
            }
        }

        /*private BattMode _battMode;
        public BattMode BattMode
        {
            get
            {
                return _battMode;
            }
            set
            {
                _battMode = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BattMode"));
                }
            }
        }*/

        public Battery(byte external, ushort vBat, ushort currIn, ushort currOut, short temp, BattState state)
        {
            OnboardExternal = external;
            Vbat = vBat;
            CurrentIn = currIn;
            CurrentOut = currOut;
            Temperture = temp;
            BattState = state;
        }


        public void Run()
        {
            while (true)
            {

            }
        }


    }
}
