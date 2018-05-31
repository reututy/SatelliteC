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
        public event PropertyChangedEventHandler PropertyChanged;

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
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("LatchupNum"));
                }
            }
        }

        public Channel(byte stat, OutputType type, ushort vol, ushort currOut, ushort latch) : base(stat, type, vol)
        {
            CurrentOut = currOut;
            LatchupNum = latch;
        }
    }
}
