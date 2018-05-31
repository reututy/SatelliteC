using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    public class EPSConfiguration : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void SetPropertyChanged(string member)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(member));
            }
        }

        private byte _pptMode;
        //!< Mode for PPT [1 = AUTO, 2 = FIXED]
        public byte PptMode
        {
            get {return _pptMode;}
            set
            {
                _pptMode = value;
                SetPropertyChanged("PptMode");
            }
        }

        private byte _battheaterMode;
        //!< Mode for battheater [0 = Manual, 1 = Auto]
        public byte BattheaterMode
        {
            get {return _battheaterMode; }
            set
            {
                _battheaterMode = value;
                SetPropertyChanged("BattheaterMode");
            }
        }

        private byte _battheaterLow;
        //!< Turn heater on at [degC]
        public byte BattheaterLow
        {
            get { return _battheaterLow; }
            set
            {
                _battheaterLow = value;
                SetPropertyChanged("BattheaterLow");
            }
        }

        private byte _battheaterHigh;
        //!< Turn heater off at [degC]
        public byte BattheaterHigh
        {
            get { return _battheaterHigh; }
            set
            {
                _battheaterHigh = value;
                SetPropertyChanged("BattheaterHigh");
            }
        } 

        private byte[] _outputNormalValue;
        //!< Nominal mode output value
        public byte[] OutputNormalValue
        {
            get { return _outputNormalValue; }
            set
            {
                _outputNormalValue = value;
                SetPropertyChanged("OutputNormalValue");
            }
        }  

        private byte[] _outputSafeValue;
        //!< Safe mode output value
        public byte[] OutputSafeValue
           {
            get { return _outputSafeValue; }
            set
            {
                _outputSafeValue = value;
                SetPropertyChanged("OutputSafeValue");
            }
        }

        private ushort[] _outputInitialOnDelay;
        //!< Output switches: init with these on delays [s]
        public ushort[] OutputInitialOnDelay
        {
            get { return _outputInitialOnDelay; }
            set
            {
                _outputInitialOnDelay = value;
                SetPropertyChanged("OutputInitialOnDelay");
            }
        } 

        private ushort[] _outputInitialOffDelay;
        //!< Output switches: init with these off delays [s]
        public ushort[] OutputInitialOffDelay
        {
            get { return _outputInitialOffDelay; }
            set
            {
                _outputInitialOffDelay = value;
                SetPropertyChanged("OutputInitialOffDelay");
            }
        }

        private ushort[] _vboost;
        //!< Fixed PPT point for boost converters [mV]
        public ushort[] Vboost
        {
            get { return _vboost; }
            set
            {
                _vboost = value;
                SetPropertyChanged("Vboost");
            }
        }

        private ushort _battMaxVoltage;
        //!< Maximum battery voltage
        public ushort BattMaxVoltage
        {
            get { return _battMaxVoltage; }
            set
            {
                _battMaxVoltage = value;
                SetPropertyChanged("BattMaxVoltage");
            }
        } 

        private ushort _battSafeVoltage;
        //!< Battery voltage for safe mode
        public ushort BattSafeVoltage
        {
            get { return _battSafeVoltage; }
            set
            {
                _battSafeVoltage = value;
                SetPropertyChanged("BattSafeVoltage");
            }
        } 

        private ushort _battCriticalVoltage;
        //!< Battery voltage for critical mode
        public ushort BattCriticalVoltage
        {
            get { return _battCriticalVoltage; }
            set
            {
                _battCriticalVoltage = value;
                SetPropertyChanged("BattCriticalVoltage");
            }
        } 

        private ushort _battNormalVoltage;
        //!< Battery voltage for normal mode
        public ushort BattNormalVoltage
        {
            get { return _battNormalVoltage; }
            set
            {
                _battNormalVoltage = value;
                SetPropertyChanged("BattNormalVoltage");
            }
        } 

        public EPSConfiguration(byte mode, byte heatMode, byte heatLow, byte heatHigh, byte[] outNormal, byte[] outSafe,
            ushort[] outOnDel, ushort[] outOffDel, ushort[] vBoost)
        {
            PptMode = mode;
            BattheaterMode = heatMode;
            BattheaterLow = heatLow;
            BattheaterHigh = heatHigh;
            OutputNormalValue = outNormal;
            OutputSafeValue = outSafe;
            OutputInitialOnDelay = outOnDel;
            OutputInitialOffDelay = outOffDel;
            Vboost = vBoost;
        }

        public void AddConfiguration2(ushort max, ushort safe, ushort critical, ushort normal)
        {
            BattMaxVoltage = max;
            BattSafeVoltage = safe;
            BattCriticalVoltage = critical;
            BattNormalVoltage = normal;
        }
    }
}
