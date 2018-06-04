using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.EPS
{
    /*channel types*/
    public enum OutputType { T_5V1, T_5V2, T_5V3, T_3_3V1, T_3_3V2, T_3_3V3, T_QS, T_QH }

    public class Output : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private byte _status;
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

        private OutputType _channelType;
        public OutputType ChannelType
        {
            get
            {
                return _channelType;
            }
            set
            {
                _channelType = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ChannelType"));
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

        public Output(byte stat, OutputType type, ushort vol)
        {
            Status = stat;
            ChannelType = type;
            Volt = vol;
        }
    }
}
