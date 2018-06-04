using DataModel.OBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class FRAMLogic
    {
        FRAM fram;

        public FRAMLogic()
        {
            fram = null;
        }

         /*!
         * Initializes the FRAM driver and the SPI driver if its not already initialized.
         * @return -2 if initializing the SPI driver fails,
         * -1 If creating semaphores to control access to the FRAM fails.
         * 0 on success.
         */
        int FRAM_start()
        {
            fram = new FRAM(OBCConstants.SIZE_8K);
            return 0;
        }

        /*!
         * De-initializes the FRAM driver.
         */
        void FRAM_stop()
        {
            fram = null;
        }

        /*!
         * Writes data to the FRAM.
         * @param data Address where data to be written is stored.
         * @param address Location in the FRAM where data should be written.
         * @param size Number of bytes to write.
         * @return -2 if the specified address and size are out of range,
         * -1 if obtaining lock for FRAM access fails,
         * 0 on success.
         */
        public int FRAM_write(byte[] data, uint address, uint size)
        {
            if (fram == null)
                return OBCConstants.ACCESS_ERROR;
            return fram.Write(data, (int)address, (int)size);
        }

         /*!
         * Reads data from the FRAM.
         * @param data Address where read data will be stored, this location must be able to accommodate size bytes.
         * @param address Location in the FRAM from which the data should be read.
         * @param size Number of bytes to read.
         * @return -2 if the specified address and size are out of range of the FRAM space.
         * -1 if obtaining lock for FRAM access fails,
         * 0 on success.
         */
        int FRAM_read(byte[] data, uint address, uint size)
        {
            if (fram == null)
                return OBCConstants.ACCESS_ERROR;
            return fram.Read(data,(int)address, (int)size);
        }
    }
}
