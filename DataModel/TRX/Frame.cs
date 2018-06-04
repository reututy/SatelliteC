using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class Frame : INotifyPropertyChanged
    {
        public ISIStrxvuRxFrame frame { get; set; }
        public ushort rx_length { get; set; } ///< Reception frame length.
        public ushort rx_doppler { get; set; } ///< Reception frame doppler measurement.
        public ushort rx_rssi { get; set; } ///< Reception frame rssi measurement.
        public byte[] rx_framedata { get; set; } ///< Reception frame data.

        public event PropertyChangedEventHandler PropertyChanged;
        private int frameId;
        public int FrameId
        {
            get { return this.frameId; }
            set
            {
                if (this.frameId != value)
                {
                    this.frameId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FrameId"));
                }
            }
        }

        public Frame()
        {
            
        }

        public Frame(ISIStrxvuRxFrame frame)
        {
            this.frame = frame;
            this.rx_length = frame.rx_length;
            this.rx_doppler = frame.rx_doppler;
            this.rx_rssi = frame.rx_rssi;
            this.rx_framedata = frame.rx_framedata;
        }

        protected void createISIStrxvuRxFrame()
        {
            ISIStrxvuRxFrame fframe = new ISIStrxvuRxFrame();
            fframe.rx_length = this.rx_length;
            fframe.rx_doppler = this.rx_doppler;
            fframe.rx_rssi = this.rx_rssi;
            fframe.rx_framedata = this.rx_framedata;
            this.frame = fframe;

        }

        public void changeRandomBits(int numberOfbits)
        {

        }

        public void changeSpecificBits(List<int> indexes)
        {

        }

    }
}
