using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace DataModel.EPS
{
    public class BoostConverter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        private ushort _volt;
        public ushort Volt
        {
            get
            {
                return _volt;
            }
            set
            {
                _volt = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Volt"));
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

        private short _fixedPPTPoint;
        public short FixedPPTPoint
        {
            get
            {
                return _fixedPPTPoint;
            }
            set
            {
                _fixedPPTPoint = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FixedPPTPoint"));
                }
            }
        }

        private bool _isSun;
        public bool IsSun
        {
            get
            {
                return _isSun;
            }
            set
            {
                _isSun = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSun"));
                }
            }
        }

        //public bool IsSunny { get; set; }

        public BoostConverter(short temp, ushort vol, ushort currIn)
        {
            Temperture = temp;
            Volt = vol;
            CurrentIn = currIn;
            IsSun = false;
        }

        /*public void run()
        {
            while (true)
            {
                if (IsSunny)
                {
                    CurrentIn += 10;//?????? change to constant
                    Volt += 10;//????????? change to constant
                    Temperture += 10;//???????? change to constant
                }
                Thread.Sleep(5000); //change to constant
            }
        }*/
    }
}
