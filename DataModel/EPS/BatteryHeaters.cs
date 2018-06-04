using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{

    /* battery heater types */
    public enum HeaterType { BP4, ONBOARD }

    public class BatteryHeater : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        /*private HeaterMode _mode;
        //0 = Manual, 1 = Auto
        public HeaterMode Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Mode"));
                }
            }
        }*/

        private HeaterType _type;
        //0 = BP4, 1= Onboard
        public HeaterType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Type"));
                }
            }
        }

        private byte _status;
        //0 = OFF 1 = ON
        public byte Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Status"));
                }
            }
        } 

        /*private sbyte _battHeaterLow;
        //! Turn heater on at [degC]
        public sbyte BattHeaterLow
        {
            get
            {
                return _battHeaterLow;
            }
            set
            {
                _battHeaterLow = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BattHeaterLow"));
                }
            }
        }   

        private sbyte _battHeaterHigh;
        //! Turn heater off at [degC]
        public sbyte BattHeaterHigh
        {
            get
            {
                return _battHeaterHigh;
            }
            set
            {
                _battHeaterHigh = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BattHeaterHigh"));
                }
            }
        }*/

        

        public BatteryHeater(HeaterType typ, byte stat)
        {
            //Mode = mod;
            Type = typ;
            Status = stat;
            //BattHeaterLow = low;
            //BattHeaterHigh = high;
        }
    }
}
