using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataModel.OBC
{
    public class FRAM
    {
        static ReaderWriterLock rwl = new ReaderWriterLock();
        public byte[] Memory { get; set; }

        public FRAM(int size)
        {
            Memory = new byte[size];
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
        public int Write(byte[] data, int address, int size)
        {
            try
            {
                rwl.AcquireWriterLock(10);
                try
                {
                    for (int i = 0; i < size; i++)
                    {
                        Memory[address + i] = data[i];
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    return OBCConstants.OUT_OF_RANGE_ERROR;
                }
                finally
                {
                    // Ensure that the lock is released.
                    rwl.ReleaseWriterLock();
                }
                return 0;
            }
            catch (ApplicationException)
            {
                // The writer lock request timed out.
                return OBCConstants.ACCESS_ERROR;
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
        public int Read(byte[] data,int address, int size)
        {
            try
            {
                rwl.AcquireReaderLock(10);
                try
                {
                    for (int i = 0; i < size; i++)
                    {
                        data[i] = Memory[address + i];
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    return OBCConstants.OUT_OF_RANGE_ERROR;
                }
                finally
                {
                    rwl.ReleaseReaderLock();
                }
                return 0;
            }
            catch (ApplicationException)
            {
                // The reader lock request timed out.
                return OBCConstants.ACCESS_ERROR;
            }
        }

    }
}
