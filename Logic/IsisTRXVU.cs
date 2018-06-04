using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using DataModel.TRX;

namespace Logic
{
    public class IsisTRXVU
    {
        

        private TRX[] tRXes;
        private List<int> errors;
        public ObservableCollection<TRX> tRXesCollection = new ObservableCollection<TRX>();

        public IsisTRXVU()
        {
            errors = new List<int>();
        }
        
        /**
         *  @brief      Initialize the ISIS TRXVU with the corresponding i2cAddress from the array of TRXVU I2C Address structure.
         *  @note       This function can only be called once.
         *  @param[in]  address array of TRXVU I2C Address structure.
         *  @param[in]  maxFrameLengths array of maximum frame length structures for TRXVU.
         *  @param[in]	default_bitrates initial default bitrate.
         *  @param[in]  number of attached TRXVU in the system to be initialized.
         *  @return     Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_initialize(ISIStrxvuI2CAddress[] address, ISIStrxvuFrameLengths[] maxFrameLengths, ISIStrxvuBitrate default_bitrates, byte number)
        {
            if(tRXes == null)
            {
                tRXes = new TRX[number];
                for(int i=0; i<number; i++)
                {
                    TRX trx = new TRX(i, address[i], maxFrameLengths[i], default_bitrates) { Name = i.ToString() };
                    tRXes[i] = trx;
                    tRXesCollection.Add(trx);
                }
                return Constants.E_NO_SS_ERR;
            }
            errors.Add(Constants.E_IS_INITIALIZED);
            return Constants.E_IS_INITIALIZED;
        }

        /**
         *  @brief       Soft Reset the ISIS TRXVU Component.
         *  @param[in]   index of ISIS TRXVU I2C bus address.
         *  @param[in]   component TRXVU component, either VU_TC or VU_RC.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_componentSoftReset(byte index, ISIStrxvuComponent component)
        {
            if(index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_componentSoftReset(component);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Hard Reset the ISIS TRXVU Component.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   component TRXVU component, either VU_TC or VU_RC.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_componentHardReset(byte index, ISIStrxvuComponent component)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_componentHardReset(component);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Soft Reset the ISIS TRXVU VU_RC and VU_TC.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_softReset(byte index)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_softReset();
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Hard Reset the ISIS TRXVU VU_RC and VU_TC.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_hardReset(byte index)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_hardReset();
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to transmit an AX.25 message with default callsigns and specified content.
         *  @param[in]   index of ISIS TRXVU I2C bus address.
         *  @param[in]   data Pointer to the array containing the data to put in the AX.25 message.
         *  @param[in]   length Length of the data to be put in the AX.25 message.
         *  @param[out]  avail Number of the available slots in the transmission buffer of the VU_TC after the frame has been added.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSendAX25DefClSign(byte index, byte[] data, byte length, Output<Byte> avail) 
        {
            if (index < tRXes.Length)
            {
                return tRXes[index].IsisTrxvu_tcSendAX25DefClSign(data, length, avail);
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to transmit an AX.25 message with override callsigns and specified content.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]	 fromCallsign This variable will define the new 7 characters from callsign.
         *  @param[in]	 toCallsign This variable will define the new 7 characters to callsign.
         *  @param[in]   data Pointer to the array containing the data to put in the AX.25 message.
         *  @param[in]   length Length of the data to be put in the AX.25 message.
         *  @param[out]  avail Number of the available slots in the transmission buffer of the VU_TC after the frame has been added.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSendAX25OvrClSign(byte index, char[] fromCallsign, char[] toCallsign, byte[] data, byte length, Output<Byte> avail)
        {
            if (index < tRXes.Length)
            {
                return tRXes[index].IsisTrxvu_tcSendAX25OvrClSign(fromCallsign, toCallsign, data, length, avail);
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to set the parameters for the AX25 Beacon with default callsigns.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   data Pointer to the array containing the message to be transmitted.
         *  @param[in]   length Length of the message.
         *  @param[in]   interval Interval of beacon transmission.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSetAx25BeaconDefClSign(byte index, byte[] data, byte length, ushort interval)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcSetAx25BeaconDefClSign(data, length, interval);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to set the parameters for the AX25 Beacon with override callsigns.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]	 fromCallsign This variable will define the new 7 characters from callsign.
         *  @param[in]	 toCallsign This variable will define the new 7 characters to callsign.
         *  @param[in]   data Pointer to the array containing the message to be transmitted.
         *  @param[in]   length Length of the message.
         *  @param[in]   interval Interval of beacon transmission.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSetAx25BeaconOvrClSign(byte index, char[] fromCallsign, char[] toCallsign, byte[] data, byte length, ushort interval)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcSetAx25BeaconOvrClSign(fromCallsign, toCallsign, data, length, interval);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to clear the current beacon.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcClearBeacon(byte index)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcClearBeacon();
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to set a new default to callsign name.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   toCallsign This variable will define the new 7 characters default to callsign.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSetDefToClSign(byte index, char[] toCallsign)
        {
            if(index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcSetDefToClSign(toCallsign);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Tell the TRXVU to set a new default from callsign name.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   fromCallsign This variable will define the new 7 characters default from callsign.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSetDefFromClSign(byte index, char[] fromCallsign)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcSetDefFromClSign(fromCallsign);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Set the idle state of the TRXVU transmitter, i.e. the state in between transmission.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   state The desired idle state of the TRXVU.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSetIdlestate(byte index, ISIStrxvuIdleState state)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcSetIdlestate(state);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Set the AX.25 bitrate of the TRXVU transmitter.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   bitrate The desired AX.25 bitrate of the TRXVU.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcSetAx25Bitrate(byte index, ISIStrxvuBitrate bitrate)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcSetAx25Bitrate(bitrate);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Retrieve the current time of operation of the TRXVU transmitter.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  uptime This array of 4 characters contains the operation time of the transmitter (Seconds, Minutes, Hours and Days, in that order).
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcGetUptime(byte index, Output<char[]> uptime)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcGetUptime(uptime);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Retrieve the current transmitter status.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  currentvutcState Pointer to the union where the current status should be stored.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_tcGetState(byte index, Output<ISIStrxvuTransmitterState> currentvutcState)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcGetState(currentvutcState);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Retrieve a block of telemetry from the TRXVU transmitter.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  telemetry Pointer to the union where the telemetry should be stored.
         *  @return      Error code according to <hal/errors.h>
         */
        int IsisTrxvu_tcGetTelemetryAll(byte index, Output<ISIStrxvuTxTelemetry> telemetry)
        {
            return 0;
        }

        /**
         *  @brief       Retrieve a block of telemetry from the TRXVU transmitter sotre on the last transmission.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  last_telemetry Pointer to the union where the telemetry should be stored.
         *  @return      Error code according to <hal/errors.h>
         */
        int IsisTrxvu_tcGetLastTxTelemetry(byte index, Output<ISIStrxvuTxTelemetry> last_telemetry)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_tcGetLastTxTelemetry(last_telemetry);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Calculates the approximate time it will take for a certain transmission to complete.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[in]   length The length of the transmission in bytes.
         *  @return      The time estimate in milliseconds.
         */
        public ushort IsisTrxvu_tcEstimateTransmissionTime(byte index, byte length)
        {
            if (index < tRXes.Length)
            {
                return tRXes[index].IsisTrxvu_tcEstimateTransmissionTime(length);
                //return Constants.E_NO_SS_ERR;
            }
            //return Constants.E_INDEX_ERROR;
            return 0;
        }

        /**
         *  @brief       Retrieve the number of telecommand frames present in the receive buffer of the TRXVU.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  frameCount The number of telecommand frames in the buffer.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_rcGetFrameCount(byte index, Output<ushort> frameCount)
        {
            tRXes[index].IsisTrxvu_rcGetFrameCount(frameCount);
            return 0;
        }

        /**
         *  @brief       Retrieve and delete a telecommand frame from the TRXVU.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  rx_frame Pointer to the struct where the telecommand frame should be store.
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_rcGetCommandFrame(byte index, Output<ISIStrxvuRxFrame> rx_frame)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_rcGetCommandFrame(rx_frame);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Retrieve a block of telemetry from the TRXVU receiver.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  telemetry Pointer to the union where the telemetry should be stored.
         *  @return      Error code according to <hal/errors.h>
         */
        int IsisTrxvu_rcGetTelemetryAll(byte index, Output<ISIStrxvuRxTelemetry> telemetry)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_rcGetTelemetryAll(telemetry);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }

        /**
         *  @brief       Retrieve the current time of operation of the TRXVU receiver.
         *  @param[in]   index index of ISIS TRXVU I2C bus address.
         *  @param[out]  uptime This array of 3 characters contains the operation time of the receiver (Minutes, Hours and Days, in that order)..
         *  @return      Error code according to <hal/errors.h>
         */
        public int IsisTrxvu_rcGetUptime(byte index, Output<char[]> uptime)
        {
            if (index < tRXes.Length)
            {
                tRXes[index].IsisTrxvu_rcGetUptime(uptime);
                return Constants.E_NO_SS_ERR;
            }
            return Constants.E_INDEX_ERROR;
        }
    }
}
