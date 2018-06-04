using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class AX25Frame : Frame
    {
        public byte FirstFlag { get; set; }
        public AX25Header Header { get; set; }
        public byte[] infoFeild; //till next iteration..
        public AX25TransferFrameInfoField InfoFeild { get; set; }
        public byte[] FrameCheckSeq { get; set; }
        public byte LastFlag { get; set; }

        public AX25Frame(char[] dest, char[] src,byte[] data)
        {
            Header = new AX25Header(dest, src);
            infoFeild = data;
            FrameCheckSeq = calculateCheckSum(data);
            rx_framedata = new byte[1 + Header.data.Length + data.Length + 16 + 1];
            rx_length = (ushort) rx_framedata.Length;
            System.Buffer.BlockCopy(Header.data, 0, rx_framedata, 1, Header.data.Length);
            System.Buffer.BlockCopy(data, 0, rx_framedata, 1 + Header.data.Length, data.Length);
            createISIStrxvuRxFrame();
        }

        public byte[] calculateCheckSum(byte[] data)
        {
            return new byte[2];
        }
    }
}
