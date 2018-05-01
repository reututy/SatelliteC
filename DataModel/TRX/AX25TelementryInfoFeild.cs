using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class AX25TelementryInfoFeild : AX25TransferFrameInfoField
    {
        private byte id;

        private byte MasterFrameCount { get; set; }
        private byte VirtualChannelFrame { get; set; }
        private byte FirstHeaderPointer { get; set; }
        private byte[] SpacePacketData { get; set; } //0-2008 , 0-251 bytes


        private byte FrameStatus;
        private String TimeFlag { get; set; }
        private String Spare2 { get; set; }
        private String TCCount { get; set; }

        private byte[] Time { get; set; } // 0-64 bits

        public int getVirtualChannelId() // 0-56
        {
            return 0;
        }

        public int getTimeFlag()
        {
            return 0;
        }

        public int getTcCount()
        {
            return 0;
        }

        public int getTime()
        {
            return 0;
        }
    }
}
