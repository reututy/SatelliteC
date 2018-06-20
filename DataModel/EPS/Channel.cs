using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    /*MSB - [QH QS 3.3V3 3.3V2 3.3V1 5V3 5V2 5V1] - LSB*/
    

    public class Channel : Output
    {
        //public event PropertyChangedEventHandler PropertyChange;

        private ushort _currentOut;
        public ushort CurrentOut
        {
            get
            {
                return _currentOut;
            }
            set
            {
                if (value >= EPSConstants.OUT_LATCHUP_PROTEC_I_MIN &&  value<= EPSConstants.OUT_LATCHUP_PROTEC_I_MAX)
                {
                    _currentOut = value;
                }
                NotifyPropertyChanged("CurrentOut");
            }
        }

        private ushort _latchupNum;
        public ushort LatchupNum
        {
            get
            {
                return _latchupNum;
            }
            set
            {
                _latchupNum = value;
                NotifyPropertyChanged("LatchupNum");
            }
        }

        public Channel(byte state, OutputType type, ushort vol, ushort currOut) : base(state, type, vol)
        {
            CurrentOut = currOut;
            LatchupNum = 0;
        }
    }
}
