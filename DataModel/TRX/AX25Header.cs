using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class AX25Header
    {
        public byte[] data { get; set; }
        public byte[] Dest { get; set; }
        public byte[] Src { get; set; }
        public byte ControlBits = Convert.ToByte(0x03);
        public byte ProtocolId = Convert.ToByte(0xF0);

        public AX25Header(byte[] dest, byte[] src)
        {
            data = new byte[16];
            // check length dest, src = 7
            System.Buffer.BlockCopy(dest, 0, data, 0, 7);
            System.Buffer.BlockCopy(src, 0, data, 7, 7);
            data[14] = ControlBits;
            data[15] = ProtocolId;
        }
    }
}
