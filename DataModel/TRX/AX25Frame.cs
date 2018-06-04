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
            int crc = 0xFFFF; // initial value
                              // loop, calculating CRC for each byte of the string
            for (int byteIndex = 0; byteIndex < data.Length; byteIndex++)
            {
                ushort bit = 0x80; // initialize bit currently being tested
                for (int bitIndex = 0; bitIndex < 8; bitIndex++)
                {
                    bool xorFlag = ((crc & 0x8000) == 0x8000);
                    crc <<= 1;
                    if (((data[byteIndex] & bit) ^ (ushort)0xff) != (ushort)0xff)
                    {
                        crc = crc + 1;
                    }
                    if (xorFlag)
                    {
                        crc = crc ^ 0x1021;
                    }
                    bit >>= 1;
                }
            }
            return BitConverter.GetBytes(crc);
        }
    }
}
