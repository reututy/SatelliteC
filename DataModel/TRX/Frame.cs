using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class Frame
    {
        public ushort rx_length { get; set; } ///< Reception frame length.
        public ushort rx_doppler { get; set; } ///< Reception frame doppler measurement.
        public ushort rx_rssi { get; set; } ///< Reception frame rssi measurement.
        public byte[] rx_framedata { get; set; } ///< Reception frame data.

        public void changeRandomBits(int numberOfbits)
        {

        }

        public void changeSpecificBits(List<int> indexes)
        {

        }

    }
}
