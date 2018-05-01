using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.TRX
{
    public class AX25TeleCommandInfoFeild : AX25TransferFrameInfoField
    {
        private byte segmentHeader = Convert.ToByte(192); //2 - 11, 6-00000
        private byte[] data;
    }
}
