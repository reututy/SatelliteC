using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.OBC
{
    public class FRAM
    {
        private byte[] memory; 

        public FRAM()
        {
            memory = new byte[OBCConstants.SIZE_8K];
        }

        /*!
         * Initializes the FRAM driver and the SPI driver if its not already initialized.
         * @return -2 if initializing the SPI driver fails,
         * -1 If creating semaphores to control access to the FRAM fails.
         * 0 on success.
         */
        /*public void Start()
        {

        }*/

        /*!
         * De-initializes the FRAM driver.
         */
        /*public void Stop()
        {

        }*/

        /*!
         * Writes data to the FRAM.
         * @param data Address where data to be written is stored.
         * @param address Location in the FRAM where data should be written.
         * @param size Number of bytes to write.
         * @return -2 if the specified address and size are out of range,
         * -1 if obtaining lock for FRAM access fails,
         * 0 on success.
         */
        public void Write(byte[] data, int address, int size)
        {
            for (int i = 0; i< size; i++)
            {
                memory[address + i] = data[i];
            }
            
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
        public byte[] Read(int address, int size)
        {
            byte[] data = new byte[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = memory[address + i];
            }
            return data;
        }

    }
}
