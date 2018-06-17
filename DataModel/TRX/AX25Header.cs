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

        public AX25Header(char[] dest, char[] src)
        {
            data = new byte[16];
            // check length dest, src = 7
            this.Dest = Encoding.UTF8.GetBytes(dest);
            this.Src = Encoding.UTF8.GetBytes(src);
            System.Buffer.BlockCopy(Dest, 0, data, 0, src.Length);
            System.Buffer.BlockCopy(Src, 0, data, 8, dest.Length);
            data[14] = ControlBits;
            data[15] = ProtocolId;
        }
    }
}
