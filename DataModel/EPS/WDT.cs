using System.ComponentModel;

namespace DataModel.EPS
{
    /*wdt types*/
    public enum WdtType { I2C, GND, CSP0, CSP1 }

    public class WDT : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private WdtType _wdtType;
        public WdtType WdtType
        {
            get
            {
                return _wdtType;
            }
            set
            {
                _wdtType = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("WdtType"));
                }
            }
        }

        private uint _rebootCounter;
        public uint RebootCounter
        {
            get
            {
                return _rebootCounter;
            }
            set
            {
                _rebootCounter = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RebootCounter"));
                }
            }
        }

        private uint _timePingLeft;
        public uint TimePingLeft
        {
            get
            {
                return _timePingLeft;
            }
            set
            {
                _timePingLeft = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("TimePingLeft"));
                }
            }
        }

        private uint _data;
        //I2C- type of reset, GND- last hour, CSP- channel connected
        public uint Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Data"));
                }
            }
        } 

        public WDT(WdtType type, uint reboot, uint left, uint dat)
        {
            WdtType = type;
            RebootCounter = reboot;
            TimePingLeft = left;
            Data = dat;
        }
    }
}
